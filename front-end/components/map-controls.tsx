"use client"

import type React from "react"
import { useState } from "react"
import { Layers, Plus, Minus, List, Search } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Tooltip, TooltipContent, TooltipProvider, TooltipTrigger } from "@/components/ui/tooltip"
import { cn } from "@/lib/utils"
import type * as L from "leaflet"

interface MapControlsProps {
  className?: string
  onTogglePanel: () => void
  panelState: "expanded" | "collapsed" | "hidden"
  propertyCount: number
  mapRef?: React.RefObject<L.Map | null>
}

export function MapControls({ className, onTogglePanel, panelState, propertyCount, mapRef }: MapControlsProps) {
  const [mapType, setMapType] = useState<"standard" | "satellite">("standard")

  const toggleMapType = () => {
    setMapType(mapType === "standard" ? "satellite" : "standard")
    // Add actual map tile layer switching logic here if needed
  }

  const handleZoomIn = () => {
    if (mapRef?.current) {
      mapRef.current.zoomIn(1)
    }
  }

  const handleZoomOut = () => {
    if (mapRef?.current) {
      mapRef.current.zoomOut(1)
    }
  }

  return (
    <div className={cn("flex flex-col gap-2", className)}>
      <TooltipProvider>
        <div className="flex flex-col gap-2 bg-background/80 backdrop-blur-sm rounded-lg shadow-lg p-1">
          <Tooltip>
            <TooltipTrigger asChild>
              <Button variant="ghost" size="icon" className="h-10 w-10" onClick={onTogglePanel} type="button">
                {panelState === "hidden" ? <Search className="h-5 w-5" /> : <List className="h-5 w-5" />}
              </Button>
            </TooltipTrigger>
            <TooltipContent side="left">
              {panelState === "hidden" ? "Show search panel" : "Toggle panel"}
            </TooltipContent>
          </Tooltip>

          <Tooltip>
            <TooltipTrigger asChild>
              <Button variant="ghost" size="icon" className="h-10 w-10" onClick={toggleMapType} type="button">
                <Layers className="h-5 w-5" />
              </Button>
            </TooltipTrigger>
            <TooltipContent side="left">
              {mapType === "standard" ? "Switch to satellite" : "Switch to standard"}
            </TooltipContent>
          </Tooltip>
        </div>

        <div className="flex flex-col gap-2 bg-background/80 backdrop-blur-sm rounded-lg shadow-lg p-1">
          <Tooltip>
            <TooltipTrigger asChild>
              <Button
                variant="ghost"
                size="icon"
                className="h-10 w-10"
                onClick={handleZoomIn}
                type="button"
                aria-label="Zoom in"
              >
                <Plus className="h-5 w-5" />
              </Button>
            </TooltipTrigger>
            <TooltipContent side="left">Zoom in</TooltipContent>
          </Tooltip>

          <Tooltip>
            <TooltipTrigger asChild>
              <Button
                variant="ghost"
                size="icon"
                className="h-10 w-10"
                onClick={handleZoomOut}
                type="button"
                aria-label="Zoom out"
              >
                <Minus className="h-5 w-5" />
              </Button>
            </TooltipTrigger>
            <TooltipContent side="left">Zoom out</TooltipContent>
          </Tooltip>
        </div>

        {panelState === "hidden" && propertyCount > 0 && (
          <div className="bg-primary text-primary-foreground rounded-full px-3 py-1 text-sm font-medium shadow-lg">
            {propertyCount}
          </div>
        )}
      </TooltipProvider>
    </div>
  )
}
