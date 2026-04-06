import http from 'k6/http';
import { check, sleep, group } from 'k6';
import { Rate, Trend } from 'k6/metrics';

// Custom metrics
const errorRate = new Rate('errors');
const loginDuration = new Trend('login_duration', true);
const bookingDuration = new Trend('booking_duration', true);
const roomsDuration = new Trend('rooms_duration', true);

// Configuration — change BASE_URL per architecture
const BASE_URL = __ENV.BASE_URL || 'http://127.0.0.1:3000';

// Load profiles — select via: k6 run --env PROFILE=constant load-test.js
const profiles = {
  constant: {
    stages: [{ duration: '5m', target: 50 }],
  },
  rampup: {
    stages: [
      { duration: '1m', target: 10 },
      { duration: '3m', target: 200 },
      { duration: '2m', target: 200 },
      { duration: '1m', target: 0 },
    ],
  },
  spike: {
    stages: [
      { duration: '1m', target: 10 },
      { duration: '30s', target: 500 },
      { duration: '1m', target: 500 },
      { duration: '30s', target: 10 },
      { duration: '2m', target: 10 },
    ],
  },
  soak: {
    stages: [
      { duration: '2m', target: 50 },
      { duration: '30m', target: 50 },
      { duration: '2m', target: 0 },
    ],
  },
};

const selectedProfile = __ENV.PROFILE || 'constant';
export const options = {
  stages: profiles[selectedProfile].stages,
  thresholds: {
    http_req_duration: ['p(95)<2000'],
    errors: ['rate<0.1'],
  },
  insecureSkipTLSVerify: true,
};

const headers = { 'Content-Type': 'application/json' };

// Each VU gets a unique user
function getUniqueEmail() {
  return `user_${__VU}_${__ITER}_${Date.now()}@test.com`;
}

export default function () {
  let token = null;
  let userId = null;
  const email = getUniqueEmail();
  const password = 'TestPass123!';

  // 1. Browse rooms (public)
  group('Browse Rooms', () => {
    const roomsRes = http.get(`${BASE_URL}/api/room`, { tags: { name: 'GET_rooms' } });
    roomsDuration.add(roomsRes.timings.duration);
    check(roomsRes, { 'rooms: status 200': (r) => r.status === 200 });
    errorRate.add(roomsRes.status !== 200);
  });

  sleep(1);

  // 2. Register
  group('Register', () => {
    const registerPayload = JSON.stringify({
      email: email,
      password: password,
      firstName: 'Load',
      lastName: 'Tester',
    });

    const registerRes = http.post(`${BASE_URL}/api/auth/register`, registerPayload, {
      headers,
      tags: { name: 'POST_register' },
    });

    check(registerRes, { 'register: status 200': (r) => r.status === 200 });
    errorRate.add(registerRes.status !== 200);

    if (registerRes.status === 200) {
      const body = JSON.parse(registerRes.body);
      token = body.token;
      userId = body.userId;
    }
  });

  sleep(1);

  // 3. Login
  group('Login', () => {
    const loginPayload = JSON.stringify({ email: email, password: password });

    const loginRes = http.post(`${BASE_URL}/api/auth/login`, loginPayload, {
      headers,
      tags: { name: 'POST_login' },
    });

    loginDuration.add(loginRes.timings.duration);
    check(loginRes, { 'login: status 200': (r) => r.status === 200 });
    errorRate.add(loginRes.status !== 200);

    if (loginRes.status === 200) {
      token = JSON.parse(loginRes.body).token;
    }
  });

  if (!token) return;

  const authHeaders = {
    'Content-Type': 'application/json',
    Authorization: `Bearer ${token}`,
  };

  sleep(1);

  // 4. Get room list + pick one for booking
  let roomId = null;
  let roomPrice = 0;
  group('Select Room', () => {
    const roomsRes = http.get(`${BASE_URL}/api/room`, { tags: { name: 'GET_rooms_select' } });

    if (roomsRes.status === 200) {
      const rooms = JSON.parse(roomsRes.body);
      const available = rooms.filter((r) => r.status === 'Available');
      if (available.length > 0) {
        const picked = available[Math.floor(Math.random() * available.length)];
        roomId = picked.roomId;
        roomPrice = picked.pricePerHour;
      }
    }
  });

  sleep(1);

  // 5. Create booking
  let bookingId = null;
  if (roomId) {
    group('Create Booking', () => {
      // Random future slot (1-72 hours ahead, on the hour)
      const hoursAhead = Math.floor(Math.random() * 72) + 1;
      const startTime = new Date();
      startTime.setHours(startTime.getHours() + hoursAhead, 0, 0, 0);

      const bookingPayload = JSON.stringify({
        roomId: roomId,
        startTime: startTime.toISOString(),
      });

      const bookingRes = http.post(`${BASE_URL}/api/booking`, bookingPayload, {
        headers: authHeaders,
        tags: { name: 'POST_booking' },
      });

      bookingDuration.add(bookingRes.timings.duration);
      check(bookingRes, {
        'booking: status 200': (r) => r.status === 200,
      });
      errorRate.add(bookingRes.status !== 200 && bookingRes.status !== 400);

      if (bookingRes.status === 200) {
        bookingId = JSON.parse(bookingRes.body).bookingId;
      }
    });
  }

  sleep(1);

  // 6. Get my bookings
  group('My Bookings', () => {
    const myRes = http.get(`${BASE_URL}/api/booking/my`, {
      headers: authHeaders,
      tags: { name: 'GET_my_bookings' },
    });

    check(myRes, { 'my bookings: status 200': (r) => r.status === 200 });
    errorRate.add(myRes.status !== 200);
  });

  sleep(1);

  // 7. Cancel booking
  if (bookingId) {
    group('Cancel Booking', () => {
      const cancelRes = http.del(`${BASE_URL}/api/booking/${bookingId}`, null, {
        headers: authHeaders,
        tags: { name: 'DELETE_booking' },
      });

      check(cancelRes, { 'cancel: status 204': (r) => r.status === 204 });
      errorRate.add(cancelRes.status !== 204);
    });
  }

  sleep(1);
}