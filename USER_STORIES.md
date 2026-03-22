# User Stories — LumeLaht Anticafe

Use this file to draft new user stories before creating them as GitHub Issues.
Copy the template at the bottom, fill it in, then create an issue.

---

## All User Stories

### Epic: Auth
| Issue | Title | SP | Priority |
|-------|-------|----|----------|
| [#10](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/10) | As a guest, I can register with email and password | 3 | High |
| [#11](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/11) | As a guest, I can log in with my credentials | 2 | High |
| [#12](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/12) | As a client, I can log out | 1 | Medium |

### Epic: Rooms
| Issue | Title | SP | Priority |
|-------|-------|----|----------|
| [#13](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/13) | As a guest, I can view a list of available rooms | 2 | High |
| [#14](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/14) | As a guest, I can filter rooms by activity type | 3 | High |
| [#15](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/15) | As a guest, I can view room details | 2 | High |
| [#26](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/26) | As an admin, I can manage rooms (CRUD) | 5 | Medium |
| [#27](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/27) | As a guest, I can see the filter panel on the left side | 2 | Medium |
| [#28](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/28) | As a user, the UI supports multiple languages (i18n) | 3 | Low |
| [#29](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/29) | As a user, I see toast notifications for actions | 2 | Low |

### Epic: Booking
| Issue | Title | SP | Priority |
|-------|-------|----|----------|
| [#16](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/16) | As a client, I can book a room for a time slot | 5 | High |
| [#17](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/17) | As a client, I can cancel my booking | 2 | Medium |
| [#18](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/18) | As a client, I can view my booking history | 3 | Medium |

### Epic: Microservices
| Issue | Title | SP | Priority |
|-------|-------|----|----------|
| [#19](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/19) | Room Service runs as an independent microservice | 5 | High |
| [#20](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/20) | Booking Service runs as an independent microservice | 5 | High |
| [#21](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/21) | API Gateway routes requests to correct services | 3 | High |
| [#22](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/22) | All microservices run via Docker Compose | 3 | High |

### Epic: Load Testing
| Issue | Title | SP | Priority |
|-------|-------|----|----------|
| [#23](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/23) | k6 load test scripts cover main user flows | 5 | High |
| [#24](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/24) | Prometheus collects metrics from all services | 3 | High |
| [#25](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/25) | Grafana dashboard visualizes architecture comparison | 3 | Medium |

---

## Summary

| Epic | Issues | Total SP |
|------|--------|----------|
| Auth | 3 | 6 |
| Rooms | 7 | 19 |
| Booking | 3 | 10 |
| Microservices | 4 | 16 |
| Load Testing | 3 | 11 |
| **Total** | **20** | **62** |

---

## Old Issues — Action Required

Close these manually in GitHub:

| Issue | Reason |
|-------|--------|
| [#2](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/2) | Superseded by #27 |
| [#3](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/3) | Not a user story — close as `not planned` |
| [#4](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/4) | Already done — close as `completed` |
| [#5](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/5) | Keep open — unit tests still relevant |
| [#6](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/6) | Already done — close as `completed` |
| [#8](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/8) | Superseded by #26 (admin CRUD) and #10-#12 (auth) |
| [#9](https://github.com/danil-polikanov/LumeLaht_Anticafe/issues/9) | Superseded by #28 (i18n) and #29 (toasts) |

---

## Labels to Create in GitHub

Go to **Issues → Labels → New label** and create these:

### Epic labels
| Name | Color |
|------|-------|
| `epic:auth` | `#0075ca` |
| `epic:rooms` | `#e4e669` |
| `epic:booking` | `#d93f0b` |
| `epic:microservices` | `#0052cc` |
| `epic:load-testing` | `#5319e7` |

### Story Points
| Name | Color |
|------|-------|
| `sp:1` | `#c2e0c6` |
| `sp:2` | `#c2e0c6` |
| `sp:3` | `#fef2c0` |
| `sp:5` | `#f9d0c4` |
| `sp:8` | `#e99695` |

### Priority
| Name | Color |
|------|-------|
| `priority:high` | `#b60205` |
| `priority:medium` | `#e4a11b` |
| `priority:low` | `#0e8a16` |

### Type
| Name | Color |
|------|-------|
| `user-story` | `#1d76db` |

---

## Milestones to Create in GitHub

Go to **Issues → Milestones → New milestone**:

| Milestone | Description | Due Date |
|-----------|-------------|----------|
| `Sprint 1 — Frontend MVP` | Auth + Rooms UI (US #10–15, #27) | — |
| `Sprint 2 — Booking` | Booking flow (US #16–18, #26) | — |
| `Sprint 3 — Microservices` | Split services + Docker (US #19–22) | — |
| `Sprint 4 — Load Testing` | k6 + Prometheus + Grafana (US #23–25) | — |

---

## Template — New User Story

Copy this when creating a new GitHub Issue:

```
## User Story
As a **[guest / client / admin / developer / researcher]**,
I want to [action],
so that [goal/benefit].

## Acceptance Criteria
- [ ] ...
- [ ] ...
- [ ] ...

**Epic:** [Auth / Rooms / Booking / Microservices / Load Testing]
**Story Points:** [1 / 2 / 3 / 5 / 8]
**Priority:** [High / Medium / Low]
```

**Labels to apply:**
- `user-story`
- `epic:*` (one of the epics above)
- `sp:*` (story points)
- `priority:*`
- `frontend` or `architecture` or `testing` (if applicable)

**Milestone:** assign to the relevant sprint.
