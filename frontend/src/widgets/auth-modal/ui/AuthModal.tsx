import React, { useEffect, useState } from 'react';
import { createPortal } from 'react-dom';
import { useLoginMutation, useRegisterMutation, setCredentials } from '@/entities/auth';
import { useAppDispatch } from '@/shared/lib/hooks/useRedux';
import toast from 'react-hot-toast';

type Tab = 'login' | 'register';

interface AuthModalProps {
  onClose: () => void;
  initialTab?: Tab;
}

export const AuthModal: React.FC<AuthModalProps> = ({ onClose, initialTab = 'login' }) => {
  const [tab, setTab] = useState<Tab>(initialTab);
  const dispatch = useAppDispatch();

  const [login, { isLoading: isLoginLoading }] = useLoginMutation();
  const [register, { isLoading: isRegisterLoading }] = useRegisterMutation();
  const isLoading = isLoginLoading || isRegisterLoading;

  const [form, setForm] = useState({
    email: '',
    password: '',
    firstName: '',
    lastName: '',
    phone: '',
  });

  const updateField = (field: string, value: string) => {
    setForm((prev) => ({ ...prev, [field]: value }));
  };

  const switchTab = (newTab: Tab) => {
    setTab(newTab);
    setForm({ email: '', password: '', firstName: '', lastName: '', phone: '' });
  };

  useEffect(() => {
    const handleKey = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose();
    };
    document.addEventListener('keydown', handleKey);
    document.body.style.overflow = 'hidden';
    return () => {
      document.removeEventListener('keydown', handleKey);
      document.body.style.overflow = '';
    };
  }, [onClose]);

  const handleBackdrop = (e: React.MouseEvent<HTMLDivElement>) => {
    if (e.target === e.currentTarget) onClose();
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      let result;
      if (tab === 'login') {
        result = await login({ email: form.email, password: form.password }).unwrap();
      } else {
        result = await register({
          email: form.email,
          password: form.password,
          firstName: form.firstName,
          lastName: form.lastName,
          phone: form.phone || undefined,
        }).unwrap();
      }
      dispatch(setCredentials(result));
      toast.success(tab === 'login' ? 'Welcome back!' : 'Account created!');
      onClose();
    } catch (err: unknown) {
      const error = err as { data?: { detail?: string; message?: string } };
      toast.error(error.data?.message || error.data?.detail || 'Something went wrong');
    }
  };

  const inputClass =
    'w-full px-4 py-3 bg-gray-50 border border-gray-200 rounded-xl text-sm focus:ring-2 focus:ring-accent/30 focus:border-accent focus:bg-white outline-none transition-all placeholder:text-gray-400';

  return createPortal(
    <div
      className="fixed inset-0 flex justify-center items-center bg-black/60 z-[9999] animate-fade-in p-4 backdrop-blur-sm"
      onClick={handleBackdrop}
    >
      <div className="bg-white rounded-2xl shadow-2xl w-full max-w-[420px] animate-scale-in overflow-hidden max-h-[90vh] overflow-y-auto">
        {/* Header */}
        <div className="bg-gradient-to-r from-accent to-accent-hover p-6 text-white text-center relative">
          <button
            onClick={onClose}
            className="absolute top-4 right-4 w-8 h-8 rounded-full flex items-center justify-center text-white hover:bg-white/20 bg-white/10 border-none cursor-pointer transition-colors"
          >
            <i className="fas fa-times text-sm"></i>
          </button>
          <div className="w-16 h-16 bg-white/20 rounded-full flex items-center justify-center mx-auto mb-3">
            <i className={`fas ${tab === 'login' ? 'fa-sign-in-alt' : 'fa-user-plus'} text-2xl`}></i>
          </div>
          <h2 className="text-xl font-bold mb-1">
            {tab === 'login' ? 'Welcome Back' : 'Create Account'}
          </h2>
          <p className="text-white/70 text-sm">
            {tab === 'login' ? 'Sign in to book your favourite rooms' : 'Join us and start booking today'}
          </p>
        </div>

        {/* Tabs */}
        <div className="flex mx-6 mt-4 bg-gray-100 rounded-xl p-1">
          <button
            type="button"
            onClick={() => switchTab('login')}
            className={`flex-1 py-2.5 text-sm font-semibold rounded-lg transition-all border-none cursor-pointer ${
              tab === 'login'
                ? 'bg-white text-accent shadow-sm'
                : 'bg-transparent text-gray-500 hover:text-gray-700'
            }`}
          >
            Login
          </button>
          <button
            type="button"
            onClick={() => switchTab('register')}
            className={`flex-1 py-2.5 text-sm font-semibold rounded-lg transition-all border-none cursor-pointer ${
              tab === 'register'
                ? 'bg-white text-accent shadow-sm'
                : 'bg-transparent text-gray-500 hover:text-gray-700'
            }`}
          >
            Register
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="p-6 flex flex-col gap-3">
          {tab === 'register' && (
            <div className="flex gap-3">
              <div className="flex-1">
                <label className="block text-xs font-medium text-gray-500 mb-1.5 uppercase tracking-wide">First Name</label>
                <input
                  type="text"
                  required
                  value={form.firstName}
                  onChange={(e) => updateField('firstName', e.target.value)}
                  className={inputClass}
                  placeholder="John"
                />
              </div>
              <div className="flex-1">
                <label className="block text-xs font-medium text-gray-500 mb-1.5 uppercase tracking-wide">Last Name</label>
                <input
                  type="text"
                  required
                  value={form.lastName}
                  onChange={(e) => updateField('lastName', e.target.value)}
                  className={inputClass}
                  placeholder="Doe"
                />
              </div>
            </div>
          )}

          {tab === 'register' && (
            <div>
              <label className="block text-xs font-medium text-gray-500 mb-1.5 uppercase tracking-wide">Phone (optional)</label>
              <input
                type="tel"
                value={form.phone}
                onChange={(e) => updateField('phone', e.target.value)}
                className={inputClass}
                placeholder="+372 5555 1234"
              />
            </div>
          )}

          <div>
            <label className="block text-xs font-medium text-gray-500 mb-1.5 uppercase tracking-wide">Email</label>
            <div className="relative">
              <i className="fas fa-envelope absolute left-3.5 top-1/2 -translate-y-1/2 text-gray-400 text-sm"></i>
              <input
                type="email"
                required
                value={form.email}
                onChange={(e) => updateField('email', e.target.value)}
                className={`${inputClass} pl-10`}
                placeholder="you@example.com"
              />
            </div>
          </div>

          <div>
            <label className="block text-xs font-medium text-gray-500 mb-1.5 uppercase tracking-wide">Password</label>
            <div className="relative">
              <i className="fas fa-lock absolute left-3.5 top-1/2 -translate-y-1/2 text-gray-400 text-sm"></i>
              <input
                type="password"
                required
                minLength={6}
                value={form.password}
                onChange={(e) => updateField('password', e.target.value)}
                className={`${inputClass} pl-10`}
                placeholder="Min 6 characters"
              />
            </div>
          </div>

          <button
            type="submit"
            disabled={isLoading}
            className="w-full bg-gradient-to-r from-accent to-accent-hover text-white py-3 rounded-xl font-semibold hover:shadow-lg hover:shadow-accent/25 transition-all disabled:opacity-50 disabled:cursor-not-allowed border-none cursor-pointer mt-1"
          >
            {isLoading ? (
              <span className="flex items-center justify-center gap-2">
                <i className="fas fa-spinner fa-spin"></i>
                {tab === 'login' ? 'Signing in...' : 'Creating account...'}
              </span>
            ) : (
              tab === 'login' ? 'Sign In' : 'Create Account'
            )}
          </button>

          <p className="text-center text-sm text-gray-400 mt-1">
            {tab === 'login' ? "Don't have an account? " : 'Already have an account? '}
            <button
              type="button"
              onClick={() => switchTab(tab === 'login' ? 'register' : 'login')}
              className="text-accent font-medium hover:underline bg-transparent border-none cursor-pointer p-0"
            >
              {tab === 'login' ? 'Register' : 'Login'}
            </button>
          </p>
        </form>
      </div>
    </div>,
    document.body
  );
};

export default AuthModal;
