'use client'

import { ThemeProvider } from 'next-themes'
import Home from "./items/page"

export default function App() {
  return (
    <ThemeProvider attribute="class">
      <div className="min-h-screen bg-background text-foreground">
        <Home />
      </div>
    </ThemeProvider>
  )
}