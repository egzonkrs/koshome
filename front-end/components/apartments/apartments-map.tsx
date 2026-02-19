"use client"

import { useEffect, useState, useRef } from "react"
import { useRouter } from "next/navigation"
import dynamic from "next/dynamic"
import type { Apartment } from "@/lib/types"
import { Skeleton } from "@/components/ui/skeleton"
import { useTheme } from "next-themes"

// Dynamically import the map components to avoid SSR issues
const MapContainer = dynamic(() => import("react-leaflet").then((mod) => mod.MapContainer), { ssr: false })
const TileLayer = dynamic(() => import("react-leaflet").then((mod) => mod.TileLayer), { ssr: false })
const Marker = dynamic(() => import("react-leaflet").then((mod) => mod.Marker), { ssr: false })
const Popup = dynamic(() => import("react-leaflet").then((mod) => mod.Popup), { ssr: false })
const ZoomControl = dynamic(() => import("react-leaflet").then((mod) => mod.ZoomControl), { ssr: false })

// Import Leaflet directly for icon configuration
import L from "leaflet"
import "leaflet/dist/leaflet.css"

interface ApartmentsMapProps {
  apartments: Apartment[]
}

export default function ApartmentsMap({ apartments }: ApartmentsMapProps) {
  const [isClient, setIsClient] = useState(false)
  const { resolvedTheme } = useTheme()
  const router = useRouter()
  const mapRef = useRef<L.Map | null>(null)
  const getLocation = (apartment: Apartment) =>
    apartment.location ?? {
      address: apartment.address || "",
      city: "Unknown",
      district: "Unknown",
      country: "Kosovo",
      coordinates: [apartment.latitude ?? 42.6629, apartment.longitude ?? 21.1655] as [number, number],
    }
  const getArea = (apartment: Apartment) => apartment.area ?? apartment.squareMeters ?? 0

  // Fix Leaflet marker icon issue in Next.js
  useEffect(() => {
    setIsClient(true)

    // Only run this code on the client side
    if (typeof window !== "undefined") {
      delete (L.Icon.Default.prototype as any)._getIconUrl
      L.Icon.Default.mergeOptions({
        iconRetinaUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon-2x.png",
        iconUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png",
        shadowUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png",
      })
    }
  }, [])

  // Custom marker icon
  const createMarkerIcon = (price: number, isSelected = false) => {
    return L.divIcon({
      html: `<div class="flex items-center justify-center w-12 h-12 rounded-full ${
        isSelected ? "bg-primary text-primary-foreground scale-125 shadow-xl" : "bg-primary text-primary-foreground"
      } font-semibold text-sm shadow-lg transition-all">${Math.round(price / 1000)}K</div>`,
      className: "map-cluster-icon",
      iconSize: L.point(48, 48),
      iconAnchor: [24, 24],
    })
  }

  // Calculate map center based on apartments
  const getMapCenter = () => {
    if (apartments.length === 0) {
      return [42.6629, 21.1655] as [number, number] // Default center (Pristina)
    }

    // Calculate average coordinates
    const sumLat = apartments.reduce((sum, apt) => sum + getLocation(apt).coordinates[0], 0)
    const sumLng = apartments.reduce((sum, apt) => sum + getLocation(apt).coordinates[1], 0)
    return [sumLat / apartments.length, sumLng / apartments.length] as [number, number]
  }

  const navigateToApartment = (id: string) => {
    router.push(`/apartments/${id}`)
  }

  if (!isClient) {
    return <Skeleton className="w-full h-full" />
  }

  return (
    <MapContainer
      center={getMapCenter()}
      zoom={13}
      minZoom={3}
      maxZoom={19}
      style={{ height: "100%", width: "100%" }}
      zoomControl={false}
      attributionControl={true}
      ref={mapRef}
      touchZoom={true}
      dragging={true}
      scrollWheelZoom={true}
      doubleClickZoom={true}
      boxZoom={true}
    >
      <TileLayer
        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>'
        url={
          resolvedTheme === "dark"
            ? "https://{s}.basemaps.cartocdn.com/dark_all/{z}/{x}/{y}{r}.png"
            : "https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png"
        }
        maxZoom={20}
        maxNativeZoom={19}
        detectRetina={true}
      />

      {/* Add custom positioned zoom control */}
      <ZoomControl position="topright" />

      {apartments.map((apartment) => {
        const location = getLocation(apartment)
        const images = apartment.images ?? []
        const area = getArea(apartment)
        const title = apartment.title || "Apartment"

        return (
          <Marker
            key={apartment.id}
            position={location.coordinates}
            icon={createMarkerIcon(apartment.price)}
            eventHandlers={{
              click: () => {
                navigateToApartment(apartment.id)
              },
            }}
          >
            <Popup className="apartment-popup">
              <div className="w-full">
                <div className="relative h-32 w-full">
                  <img
                    src={images[0] || "/placeholder.svg"}
                    alt={title}
                    className="w-full h-full object-cover"
                  />
                  <div className="absolute bottom-0 left-0 bg-primary text-primary-foreground px-2 py-1 text-sm font-semibold">
                    {apartment.price.toLocaleString()} €
                  </div>
                </div>
                <div className="p-3">
                  <h3
                    className="font-semibold text-sm truncate cursor-pointer hover:text-primary"
                    onClick={() => navigateToApartment(apartment.id)}
                  >
                    {title}
                  </h3>
                  <p className="text-xs text-muted-foreground truncate">
                    {location.district}, {location.city}
                  </p>
                  <div className="flex items-center gap-2 mt-2 text-xs">
                    <span>{apartment.bedrooms} BD</span>
                    <span>{apartment.bathrooms} BA</span>
                    <span>{area} m²</span>
                  </div>
                </div>
              </div>
            </Popup>
          </Marker>
        )
      })}
    </MapContainer>
  )
}
