// app/user/register/page.tsx
"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { authService } from "@/services/authService";
import { User, Mail, Lock, ArrowRight } from 'lucide-react';

export default function RegisterPage() {
  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await authService.register(username, email, password);
      router.push("user/login");
    } catch (err) {
      setError("Registration failed. Please try again.");
    }
  };

  return (
    <div className="auth-container">
    <div className="auth-form-container glass-card">
      <h2 className="auth-title gradient-text">Join Us</h2>
      <p className="text-center text-muted-foreground mb-8">Create your account</p>
      <form className="auth-form space-y-6" onSubmit={handleSubmit}>
        <div className="space-y-4">
          <div className="relative">
            <User className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground" size={20} />
            <input
              id="username"
              name="username"
              type="text"
              required
              className="auth-input pl-10"
              placeholder="Username"
              value={username}
              onChange={(e) => setUsername(e.target.value)}
            />
          </div>
          <div className="relative">
            <Mail className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground" size={20} />
            <input
              id="email-address"
              name="email"
              type="email"
              autoComplete="email"
              required
              className="auth-input pl-10"
              placeholder="Email address"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </div>
          <div className="relative">
            <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground" size={20} />
            <input
              id="password"
              name="password"
              type="password"
              autoComplete="new-password"
              required
              className="auth-input pl-10"
              placeholder="Password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
            />
          </div>
        </div>

        {error && <div className="text-destructive text-sm text-center">{error}</div>}

        <div>
          <button type="submit" className="auth-submit-button soft-glow group">
            Create Account
            <ArrowRight className="inline-block ml-2 transition-transform group-hover:translate-x-1" size={20} />
          </button>
        </div>
      </form>
      <div className="mt-6 text-center">
        <p className="text-sm text-muted-foreground">
          Already have an account?{' '}
          <a href="#" className="text-primary hover:underline">Sign in</a>
        </p>
      </div>
    </div>
  </div>
  );
}
