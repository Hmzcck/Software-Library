"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { authService } from "@/services/authService";
import { User, Lock, ArrowRight } from 'lucide-react';

export default function LoginPage() {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const router = useRouter();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await authService.login(username, password);
      router.push("/");
      router.refresh();
    } catch (err) {
      setError("Login failed. Please check your credentials.");
    }
  };

  return (
    <div className="auth-container">
      <div className="auth-form-container glass-card">
        <h2 className="auth-title gradient-text">Welcome Back</h2>
        <p className="text-center text-muted-foreground mb-8">Sign in to your account</p>
        <form className="auth-form space-y-6" onSubmit={handleSubmit}>
          <div className="space-y-4">
            <div className="relative">
              <User className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground" size={20} />
              <input
                id="username"
                name="username"
                type="text"
                autoComplete="username"
                required
                className="auth-input pl-10"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>
            <div className="relative">
              <Lock className="absolute left-3 top-1/2 transform -translate-y-1/2 text-muted-foreground" size={20} />
              <input
                id="password"
                name="password"
                type="password"
                autoComplete="current-password"
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
              Sign in
              <ArrowRight className="inline-block ml-2 transition-transform group-hover:translate-x-1" size={20} />
            </button>
          </div>
        </form>
        <div className="mt-6 text-center">
          <a href="#" className="text-sm text-primary hover:underline">Forgot your password?</a>
        </div>
      </div>
    </div>
  );
}
