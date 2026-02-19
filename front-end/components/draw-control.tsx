"use client"

import { useEffect, useRef } from "react"
import { useMap } from "react-leaflet"
import L from "leaflet"
import "leaflet-draw"
import "leaflet-draw/dist/leaflet.draw.css"
import { useApartmentStore } from "@/store/apartment-store"

interface DrawControlProps {
  isDrawingEnabled: boolean
  onDrawComplete?: (bounds: [number, number][]) => void
  onDrawClear?: () => void
}

export function DrawControl({ isDrawingEnabled, onDrawComplete, onDrawClear }: DrawControlProps) {
  const map = useMap()
  const { setFilters } = useApartmentStore()
  const drawnItemsRef = useRef<L.FeatureGroup | null>(null)
  const drawControlRef = useRef<L.Control.Draw | null>(null)

  useEffect(() => {
    if (!map) return

    // Initialize the FeatureGroup for drawn items
    if (!drawnItemsRef.current) {
      drawnItemsRef.current = new L.FeatureGroup()
      map.addLayer(drawnItemsRef.current)
    }

    // Create draw control
    if (!drawControlRef.current) {
      drawControlRef.current = new L.Control.Draw({
        position: "topright",
        draw: {
          polyline: false,
          circle: false,
          circlemarker: false,
          marker: false,
          rectangle: {
            shapeOptions: {
              color: "#22c55e",
              weight: 2,
              fillOpacity: 0.1,
              fillColor: "#22c55e",
            },
          },
          polygon: {
            allowIntersection: false,
            showArea: true,
            shapeOptions: {
              color: "#22c55e",
              weight: 2,
              fillOpacity: 0.1,
              fillColor: "#22c55e",
            },
          },
        },
        edit: {
          featureGroup: drawnItemsRef.current,
          remove: true,
        },
      })
    }

    // Handle draw created event
    const onDrawCreated = (e: L.LeafletEvent) => {
      const event = e as L.DrawEvents.Created
      const layer = event.layer

      // Clear previous drawings
      drawnItemsRef.current?.clearLayers()

      // Add new layer
      drawnItemsRef.current?.addLayer(layer)

      // Get polygon/rectangle bounds
      let bounds: [number, number][] = []

      if (layer instanceof L.Polygon || layer instanceof L.Rectangle) {
        const latLngs = layer.getLatLngs()[0] as L.LatLng[]
        bounds = latLngs.map((latLng) => [latLng.lat, latLng.lng] as [number, number])
      }

      // Update filter with polygon bounds
      if (bounds.length > 0) {
        setFilters({ polygonBounds: bounds })
        onDrawComplete?.(bounds)
      }
    }

    // Handle draw deleted event
    const onDrawDeleted = () => {
      setFilters({ polygonBounds: undefined })
      onDrawClear?.()
    }

    // Handle draw edited event
    const onDrawEdited = (e: L.LeafletEvent) => {
      const event = e as L.DrawEvents.Edited
      const layers = event.layers

      layers.eachLayer((layer) => {
        if (layer instanceof L.Polygon || layer instanceof L.Rectangle) {
          const latLngs = layer.getLatLngs()[0] as L.LatLng[]
          const bounds = latLngs.map((latLng) => [latLng.lat, latLng.lng] as [number, number])
          if (bounds.length > 0) {
            setFilters({ polygonBounds: bounds })
            onDrawComplete?.(bounds)
          }
        }
      })
    }

    // Add event listeners
    map.on(L.Draw.Event.CREATED, onDrawCreated)
    map.on(L.Draw.Event.DELETED, onDrawDeleted)
    map.on(L.Draw.Event.EDITED, onDrawEdited)

    // Cleanup
    return () => {
      map.off(L.Draw.Event.CREATED, onDrawCreated)
      map.off(L.Draw.Event.DELETED, onDrawDeleted)
      map.off(L.Draw.Event.EDITED, onDrawEdited)
    }
  }, [map, setFilters, onDrawComplete, onDrawClear])

  // Toggle draw control visibility
  useEffect(() => {
    if (!map || !drawControlRef.current) return

    if (isDrawingEnabled) {
      map.addControl(drawControlRef.current)
    } else {
      try {
        map.removeControl(drawControlRef.current)
      } catch {
        // Control might not be on map
      }
    }

    return () => {
      if (drawControlRef.current) {
        try {
          map.removeControl(drawControlRef.current)
        } catch {
          // Control might not be on map
        }
      }
    }
  }, [map, isDrawingEnabled])

  // Clear drawings when disabled
  useEffect(() => {
    if (!isDrawingEnabled && drawnItemsRef.current) {
      drawnItemsRef.current.clearLayers()
      setFilters({ polygonBounds: undefined })
    }
  }, [isDrawingEnabled, setFilters])

  return null
}
