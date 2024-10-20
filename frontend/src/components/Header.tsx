// app/components/Header.jsx
"use client";

import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Button from "./Button";
import Categories from "./Categories";
import { authService } from "@/services/authService";
import ThemeToggle from "@/components/ThemeToggle";

export default function Header() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const router = useRouter();

  useEffect(() => {
    setIsLoggedIn(authService.isLoggedIn());
  }, []);

  const handleLogout = async () => {
    await authService.logout();
    setIsLoggedIn(false);
    router.push("/");
  };

  return (
    <header className="header">
      <div className="header-content">
        <div className="flex items-center mb-3">
          <img src="/logo.png" alt="Logo" className="header-logo" />
        </div>

        <div className="header-search">
          <input
            type="text"
            className="header-search-input"
            id="Search"
            placeholder="Search for a software..."
          />
          <label htmlFor="Search" className="header-search-label">
            Search for a software...
          </label>
        </div>

        <div className="header-buttons">
          {!isLoggedIn ? (
            <>
              <Link href="/user/login">
                <Button
                  variant="header"
                  label="Login"
                  onClick={() => console.log("Login clicked")}
                />
              </Link>
              <Link href="/user/register">
                <Button
                  variant="header"
                  label="Register"
                  onClick={() => console.log("Register clicked")}
                />
              </Link>
            </>
          ) : (
            <Button variant="header" label="Logout" onClick={handleLogout} />
          )}
        </div>
      </div>

      <div className="relative w-full">
        <div className="absolute left-0 flex">
          <Categories />
          <div className="mr-5"> </div>
          <Link href="/">
            <Button
              variant="header"
              label="Home"
              onClick={() => console.log("Home clicked")}
            />
          </Link>
        </div>

        <div className="flex justify-center">
          <div className="flex gap-x-10">
            <Link href="/items?sort=stars">
              <Button
                variant="header"
                label="Most Stars"
                onClick={() => console.log("Most Stars clicked")}
              />
            </Link>
            <Link href="/items?sort=forks">
              <Button
                variant="header"
                label="Most Forks"
                onClick={() => console.log("Most Forks clicked")}
              />
            </Link>
            <Link href="/items?sort=new">
              <Button
                variant="header"
                label="New Softwares"
                onClick={() => console.log("New Softwares clicked")}
              />
            </Link>
          </div>

          {isLoggedIn && (
            <div className="absolute right-0">
              <Link href="/user/favoriteItems">
                <Button
                  variant="header"
                  label="Favorite Items"
                  onClick={() => console.log("Favorite Items clicked")}
                />
              </Link>
            </div>
          )}
          <div className="absolute right-40">
            <ThemeToggle />
          </div>
        </div>
      </div>
    </header>
  );
}
