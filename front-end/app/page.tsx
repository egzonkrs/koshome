"use client"

import { useState, useEffect, useRef } from "react"
import dynamic from "next/dynamic"
import { Header } from "@/components/header"
import { FloatingPanel } from "@/components/floating-panel"
import { MapControls } from "@/components/map-controls"
import { useApartmentStore } from "@/store/apartment-store"
import { cn } from "@/lib/utils"
import { useApartments } from "@/lib/query"
// Add the import for Leaflet
import type L from "leaflet"

// Dynamically import the map component to avoid SSR issues with Leaflet
const ApartmentMap = dynamic(() => import("@/components/map"), {
  ssr: false,
  loading: () => (
    <div className="h-full w-full flex items-center justify-center bg-muted">
      <p>Loading map...</p>
    </div>
  ),
})

export default function Home() {
  const [mounted, setMounted] = useState(false)
  const [panelState, setPanelState] = useState<"expanded" | "collapsed" | "hidden">("expanded")
  const [panelPosition, setPanelPosition] = useState<"left" | "bottom">("bottom")
  const mapRef = useRef<HTMLDivElement>(null)
  const leafletMapRef = useRef<L.Map | null>(null)
  const { filteredApartments, filters, setApartments } = useApartmentStore()
  const { data } = useApartments(filters)

  // Fix hydration issues
  useEffect(() => {
    setMounted(true)
  }, [])

  useEffect(() => {
    const items = data?.data?.items
    if (data?.isSuccess && items) {
      setApartments(items)
    }
  }, [data, setApartments])

  // Determine panel position based on screen size
  useEffect(() => {
    const handleResize = () => {
      setPanelPosition(window.innerWidth >= 1024 ? "left" : "bottom")
    }

    handleResize()
    window.addEventListener("resize", handleResize)
    return () => window.removeEventListener("resize", handleResize)
  }, [])

  // Toggle panel state
  const togglePanel = () => {
    setPanelState((state) => {
      if (state === "expanded") return "collapsed"
      if (state === "collapsed") return "hidden"
      return "expanded"
    })
  }

  // Show panel when hidden
  const showPanel = () => {
    if (panelState === "hidden") {
      setPanelState("expanded")
    }
  }

  if (!mounted) {
    return null
  }

  return (
    <div className="flex flex-col h-screen">
      <Header />

      <main className="flex-1 relative overflow-hidden">
        {/* Full-screen map */}
        <div ref={mapRef} className="absolute inset-0 z-10">
          <ApartmentMap onMapClick={showPanel} mapRef={leafletMapRef} />
        </div>

        {/* Map controls */}
        <MapControls
          className={cn(
            "absolute z-30 transition-all duration-300",
            panelPosition === "left" ? "top-4 right-4" : "top-4 right-4",
            panelState === "expanded" && panelPosition === "left" ? "lg:right-4" : "",
          )}
          onTogglePanel={togglePanel}
          panelState={panelState}
          propertyCount={filteredApartments.length}
          mapRef={leafletMapRef}
        />

        {/* Floating panel with listings */}
        <FloatingPanel state={panelState} position={panelPosition} onStateChange={setPanelState} />
      </main>
    </div>
  )
}
