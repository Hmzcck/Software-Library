// app/components/Header.jsx
"use client";

import React, { useState, useEffect } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import Button from "./Button";
import Categories from "./Categories";
import { authService } from "@/services/authService";

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
    <header className="w-full shadow-md bg-background">
    <div className="flex items-center justify-between p-4 bg-background">
      <div className="flex items-center mb-3">
        <img src="/logo.png" alt="Logo" className="h-12 w-auto" />
      </div>

      <div className="relative mb-3 ml-36 w-1/2 justify-center">
        <input
          type="text"
          className="peer block min-h-[auto] w-full bg-transparent px-3 py-[0.32rem] leading-[1.6] outline-none transition-all duration-200 ease-linear focus:placeholder:opacity-100 peer-focus:text-primary rounded border-2 border-primary"
          id="Search"
          placeholder="Search for a software..."
        />
        <label
          htmlFor="Search"
          className="pointer-events-none absolute left-3 top-0 mb-0 max-w-[90%] origin-[0_0] truncate pt-[0.37rem] leading-[1.6] text-neutral-500 transition-all duration-200 ease-out peer-focus:-translate-y-[1.5rem] peer-focus:scale-[0.8] peer-focus:text-primary"
        >
          Search for a software...
        </label>
      </div>

      <div className="flex items-center space-x-10 mb-3 mr-5">
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
      <div className = "mr-5"> </div>
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
          <Button
            variant="header"
            label="Most Stars"
            onClick={() => console.log("Most Stars clicked")}
          />
          <Button
            variant="header"
            label="Most Forks"
            onClick={() => console.log("Most Forks clicked")}
          />
          <Button
            variant="header"
            label="New Softwares"
            onClick={() => console.log("New Softwares clicked")}
          />
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
      </div>
    </div>
  </header>
  );
}
