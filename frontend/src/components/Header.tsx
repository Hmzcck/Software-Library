"use client";

import React from "react";
import Button from "./Button";

export default function Header(): JSX.Element {
  return (
    <header className="w-full shadow-md bg-background">
      <div className="flex items-center justify-between p-4 bg-background">
        <div className="flex items-center mb-3">
          <img src="/logo.png" alt="Logo" className="h-12 w-auto" />
        </div>

        <div className="relative mb-3 ml-36 w-1/2 justify-center">
          <input
            type="text"
            className="peer block min-h-[auto] w-full bg-transparent px-3 py-[0.32rem] leading-[1.6] outline-none transition-all duration-200 ease-linear focus:placeholder:opacity-100 peer-focus:text-primary data-[twe-input-state-active]:placeholder:opacity-100 motion-reduce:transition-none dark:text-white dark:placeholder:text-neutral-300 dark:autofill:shadow-autofill dark:peer-focus:text-primary [&:not([data-twe-input-placeholder-active])]:placeholder:opacity-0 rounded border-2 border-primary"
            id="Search"
            placeholder="Example label"
          />
          <label
            htmlFor="Search"
            className="pointer-events-none absolute left-3 top-0 mb-0 max-w-[90%] origin-[0_0] truncate pt-[0.37rem] leading-[1.6] text-neutral-500 transition-all duration-200 ease-out peer-focus:-translate-y-[1.5rem] peer-focus:scale-[0.8] peer-focus:text-primary peer-data-[twe-input-state-active]:-translate-y-[1.5rem] peer-data-[twe-input-state-active]:scale-[0.8] motion-reduce:transition-none dark:text-neutral-400 dark:peer-focus:text-primary"
          >
            Search for a software...
          </label>
        </div>

        <div className="flex items-center space-x-10 mb-3 mr-5">
          <Button
            variant="header"
            label="Login"
            onClick={() => console.log("Login clicked")}
          />
          <Button
            variant="header"
            label="Register"
            onClick={() => console.log("Register clicked")}
          />
        </div>
      </div>

      <div className="flex justify-between items-center">
        {/* Left-aligned Categories button */}
        <Button
          variant="header"
          label="Categories"
          onClick={() => console.log("Categories clicked")}
        />

        {/* Center-aligned Most Stars and Most Downloads buttons */}
        <div className="flex gap-x-10 justify-center w-full mr-60">
          <Button
            variant="header"
            label="Most Stars"
            onClick={() => console.log("Most Stars clicked")}
          />
          <Button
            variant="header"
            label="Most Downloads"
            onClick={() => console.log("Most Downloads clicked")}
          />
             <Button
            variant="header"
            label="New Softwares"
            onClick={() => console.log("New Softwares clicked")}
          />
        </div>
      </div>
    </header>
  );
}
