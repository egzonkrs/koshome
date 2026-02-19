"use client"

import type React from "react"
import { useEffect, useState, useRef } from "react"
import { useRouter } from "next/navigation"
import dynamic from "next/dynamic"
import { useApartmentStore } from "@/store/apartment-store"
import type { Apartment } from "@/lib/types"
import { useTheme } from "next-themes"
import { Skeleton } from "@/components/ui/skeleton"
import { Button } from "@/components/ui/button"
import { Sparkles, TrendingUp, MapPin, Lightbulb, X, ChevronRight, Pencil, Eraser } from "lucide-react"

// Dynamically import the map components to avoid SSR issues
const MapContainer = dynamic(() => import("react-leaflet").then((mod) => mod.MapContainer), { ssr: false })
const TileLayer = dynamic(() => import("react-leaflet").then((mod) => mod.TileLayer), { ssr: false })
const Marker = dynamic(() => import("react-leaflet").then((mod) => mod.Marker), { ssr: false })
const Popup = dynamic(() => import("react-leaflet").then((mod) => mod.Popup), { ssr: false })
const Tooltip = dynamic(() => import("react-leaflet").then((mod) => mod.Tooltip), { ssr: false })
const ZoomControl = dynamic(() => import("react-leaflet").then((mod) => mod.ZoomControl), { ssr: false })
const DrawControlComponent = dynamic(() => import("@/components/draw-control").then((mod) => mod.DrawControl), { ssr: false })

// Import Leaflet directly for icon configuration
import L from "leaflet"
import "leaflet/dist/leaflet.css"
import { useMap } from "react-leaflet"

const getApartmentLocation = (apartment: Apartment) =>
  apartment.location ?? {
    address: apartment.address || "",
    city: "Unknown",
    district: "Unknown",
    country: "Kosovo",
    coordinates: [apartment.latitude ?? 42.6629, apartment.longitude ?? 21.1655] as [number, number],
  }

const getApartmentArea = (apartment: Apartment) => apartment.area ?? apartment.squareMeters ?? 0
const getApartmentImages = (apartment: Apartment) => apartment.images ?? []

// Dynamically import the MapController component to avoid SSR issues with useMap
const MapControllerComponent = dynamic(
  () =>
    Promise.resolve(({ apartment }: { apartment: Apartment | null }) => {
      // eslint-disable-next-line react-hooks/rules-of-hooks
      const map = useMap()

      // eslint-disable-next-line react-hooks/rules-of-hooks
      useEffect(() => {
        if (apartment) {
          map.setView(getApartmentLocation(apartment).coordinates, 15)
        }
      }, [apartment, map])

      return null
    }),
  { ssr: false },
)

const mockAIResponses = {
  marketAnalysis: [
    "Based on current market trends, this area has seen a 12% price increase over the past year.",
    "Properties in this district typically sell within 45 days of listing.",
    "The average price per m² in this neighborhood is €1,450, making this property competitively priced.",
    "Market demand in this area is HIGH - expect multiple offers on desirable properties.",
  ],
  neighborhoodInsights: [
    "This neighborhood has excellent walkability with a score of 85/100.",
    "Nearby amenities include 3 schools, 2 parks, and multiple shopping centers within 1km.",
    "Public transport access is excellent with bus and tram stops within 200m.",
    "Crime rates in this area are 25% below the city average.",
  ],
  investmentTips: [
    "Rental yield potential: 5.2% annually based on comparable properties.",
    "Property values in this district are projected to increase 8-10% over the next 2 years.",
    "This property type has historically outperformed the market by 15%.",
    "Consider: nearby infrastructure projects may boost property values further.",
  ],
  priceEstimate: [
    "AI Estimated Value: €118,500 - €125,000",
    "This property is priced 3% below market average for similar units.",
    "Comparable sales in the last 90 days: €115,000 - €130,000",
    "Price confidence level: HIGH (based on 12 comparable properties)",
  ],
}

interface ApartmentMapProps {
  onMapClick?: () => void
  mapRef?: React.RefObject<L.Map | null>
}

export default function ApartmentMap({ onMapClick, mapRef }: ApartmentMapProps) {
  const { filteredApartments, selectedApartment, setSelectedApartment } = useApartmentStore()
  const { resolvedTheme } = useTheme()
  const [isClient, setIsClient] = useState(false)
  const router = useRouter()
  const localMapRef = useRef<L.Map | null>(null)
  const activeMapRef = mapRef ?? localMapRef

  const [showAIPanel, setShowAIPanel] = useState(false)
  const [aiLoading, setAiLoading] = useState(false)
  const [aiInsight, setAiInsight] = useState<{ type: string; content: string[] } | null>(null)
  const [activeAIFeature, setActiveAIFeature] = useState<string | null>(null)
  const [isDrawingMode, setIsDrawingMode] = useState(false)

  // Fix Leaflet marker icon issue in Next.js
  useEffect(() => {
    setIsClient(true)

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
  const createMarkerIcon = (price: number, theme: string, isSelected: boolean) => {
    return L.divIcon({
      html: `<div class="flex items-center justify-center w-12 h-12 rounded-full ${
        isSelected
          ? "bg-primary text-primary-foreground scale-125 shadow-xl"
          : theme === "dark"
            ? "bg-primary text-primary-foreground"
            : "bg-primary text-primary-foreground"
      } font-semibold text-sm shadow-lg transition-all">${Math.round(price / 1000)}K</div>`,
      className: "map-cluster-icon",
      iconSize: L.point(48, 48),
      iconAnchor: [24, 24],
    })
  }

  // Calculate map center based on apartments
  const getMapCenter = () => {
    if (selectedApartment) {
      return getApartmentLocation(selectedApartment).coordinates
    }

    if (filteredApartments.length === 0) {
      return [42.6629, 21.1655] as [number, number] // Default center (Pristina)
    }

    // Calculate average coordinates
    const sumLat = filteredApartments.reduce((sum, apt) => sum + getApartmentLocation(apt).coordinates[0], 0)
    const sumLng = filteredApartments.reduce((sum, apt) => sum + getApartmentLocation(apt).coordinates[1], 0)
    return [sumLat / filteredApartments.length, sumLng / filteredApartments.length] as [number, number]
  }

  const handleApartmentClick = (apartment: Apartment) => {
    setSelectedApartment(apartment)
  }

  const navigateToApartment = (id: string) => {
    router.push(`/apartments/${id}`)
  }

  const handleMapClick = () => {
    if (onMapClick) {
      onMapClick()
    }
  }

  useEffect(() => {
    const map = activeMapRef.current
    if (!map) return

    map.on("click", handleMapClick)
    return () => {
      map.off("click", handleMapClick)
    }
  }, [activeMapRef, handleMapClick])

  const handleAIFeature = (featureType: string) => {
    setAiLoading(true)
    setActiveAIFeature(featureType)
    setShowAIPanel(true)

    // Simulate AI processing delay
    setTimeout(() => {
      const responses = mockAIResponses[featureType as keyof typeof mockAIResponses] || []
      setAiInsight({ type: featureType, content: responses })
      setAiLoading(false)
    }, 1500)
  }

  const getAIFeatureTitle = (type: string) => {
    switch (type) {
      case "marketAnalysis":
        return "Market Analysis"
      case "neighborhoodInsights":
        return "Neighborhood Insights"
      case "investmentTips":
        return "Investment Tips"
      case "priceEstimate":
        return "AI Price Estimate"
      default:
        return "AI Insights"
    }
  }

  const getAIFeatureIcon = (type: string) => {
    switch (type) {
      case "marketAnalysis":
        return <TrendingUp className="h-4 w-4" />
      case "neighborhoodInsights":
        return <MapPin className="h-4 w-4" />
      case "investmentTips":
        return <Lightbulb className="h-4 w-4" />
      case "priceEstimate":
        return <Sparkles className="h-4 w-4" />
      default:
        return <Sparkles className="h-4 w-4" />
    }
  }

  if (!isClient) {
    return <Skeleton className="h-full w-full" />
  }

  return (
    <div className="h-full w-full relative">
      <MapContainer
        center={getMapCenter()}
        zoom={13}
        minZoom={3}
        maxZoom={19}
        style={{ height: "100%", width: "100%" }}
        ref={activeMapRef}
        zoomControl={false}
        attributionControl={false}
        touchZoom={true}
        dragging={true}
        scrollWheelZoom={true}
        doubleClickZoom={true}
        boxZoom={true}
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>'
          url="https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png"
          maxZoom={20}
          maxNativeZoom={19}
          detectRetina={true}
        />

        {/* Add custom positioned zoom control */}
        <ZoomControl position="topright" />

        {filteredApartments.map((apartment) => {
          const location = getApartmentLocation(apartment)
          const images = getApartmentImages(apartment)
          const area = getApartmentArea(apartment)
          const title = apartment.title || "Apartment"

          return (
            <Marker
              key={apartment.id}
              position={location.coordinates}
              icon={createMarkerIcon(apartment.price, resolvedTheme || "light", selectedApartment?.id === apartment.id)}
              eventHandlers={{
                click: () => {
                  handleApartmentClick(apartment)
                },
              }}
            >
              <Tooltip direction="top" offset={[0, -20]} opacity={1} permanent={false}>
                <div className="font-semibold text-sm text-primary">{apartment.price.toLocaleString()} €</div>
                <div className="text-xs text-muted-foreground mt-0.5">
                  {apartment.bedrooms} BD · {apartment.bathrooms} BA · {area} m²
                </div>
              </Tooltip>
              <Popup className="apartment-popup">
                <div className="w-full">
                  <div className="relative h-36 w-full overflow-hidden">
                    <img
                      src={images[0] || "/placeholder.svg"}
                      alt={title}
                      className="w-full h-full object-cover"
                    />
                    <div className="absolute bottom-0 left-0 right-0 bg-gradient-to-t from-black/70 to-transparent p-3">
                      <div className="text-white font-bold text-lg drop-shadow-md">
                        {apartment.price.toLocaleString()} €
                      </div>
                    </div>
                  </div>
                  <div className="p-4">
                    <h3
                      className="font-semibold text-base truncate cursor-pointer hover:text-primary transition-colors"
                      onClick={() => navigateToApartment(apartment.id)}
                    >
                      {title}
                    </h3>
                    <p className="text-xs text-muted-foreground truncate mt-1 flex items-center gap-1">
                      <MapPin className="h-3 w-3" />
                      {location.district}, {location.city}
                    </p>
                    <div className="flex items-center gap-3 mt-3 text-xs text-muted-foreground">
                      <span className="flex items-center gap-1">
                        <span className="font-medium text-foreground">{apartment.bedrooms}</span> BD
                      </span>
                      <span className="flex items-center gap-1">
                        <span className="font-medium text-foreground">{apartment.bathrooms}</span> BA
                      </span>
                      <span className="flex items-center gap-1">
                        <span className="font-medium text-foreground">{area}</span> m²
                      </span>
                    </div>
                    <Button size="sm" className="w-full mt-3" onClick={() => navigateToApartment(apartment.id)}>
                      View Details
                      <ChevronRight className="h-4 w-4 ml-1" />
                    </Button>
                  </div>
                </div>
              </Popup>
            </Marker>
          )
        })}

        <MapControllerComponent apartment={selectedApartment} />
        <DrawControlComponent isDrawingEnabled={isDrawingMode} />
      </MapContainer>

      {/* Map Tools - Bottom Right FAB Menu */}
      <div className="absolute bottom-20 md:bottom-6 right-4 z-20 flex flex-col items-end gap-2">
        {/* Draw Mode Active Tooltip */}
        {isDrawingMode && (
          <div className="p-3 bg-card/95 backdrop-blur-sm rounded-xl border shadow-lg text-xs text-muted-foreground max-w-[200px] animate-in slide-in-from-right-2">
            <p className="font-semibold text-foreground mb-1 flex items-center gap-1">
              <Pencil className="h-3 w-3" />
              Draw Mode Active
            </p>
            <p>Draw a polygon or rectangle on the map to filter properties.</p>
          </div>
        )}

        {/* Tool Buttons - Horizontal on desktop, vertical stack on mobile */}
        <div className="flex flex-col md:flex-row gap-2">
          {/* Draw to Search Button */}
          <Button
            size="sm"
            variant={isDrawingMode ? "default" : "secondary"}
            className={`shadow-lg backdrop-blur-sm border h-10 w-10 md:w-auto md:h-auto md:px-3 p-0 rounded-full md:rounded-lg ${
              isDrawingMode 
                ? "bg-primary text-primary-foreground hover:bg-primary/90" 
                : "bg-card/95 hover:bg-card"
            }`}
            onClick={() => setIsDrawingMode(!isDrawingMode)}
            title={isDrawingMode ? "Exit Draw Mode" : "Draw to Search"}
          >
            {isDrawingMode ? (
              <>
                <Eraser className="h-4 w-4 md:mr-2" />
                <span className="hidden md:inline">Exit Draw</span>
              </>
            ) : (
              <>
                <Pencil className="h-4 w-4 md:mr-2 md:text-primary" />
                <span className="hidden md:inline">Draw Area</span>
              </>
            )}
          </Button>

          {/* AI Features Buttons */}
          <Button
            size="sm"
            variant="secondary"
            className="shadow-lg bg-card/95 backdrop-blur-sm hover:bg-card border h-10 w-10 md:w-auto md:h-auto md:px-3 p-0 rounded-full md:rounded-lg"
            onClick={() => handleAIFeature("marketAnalysis")}
            title="Market Analysis"
          >
            <TrendingUp className="h-4 w-4 md:mr-2 text-primary" />
            <span className="hidden md:inline">Market</span>
          </Button>
          <Button
            size="sm"
            variant="secondary"
            className="shadow-lg bg-card/95 backdrop-blur-sm hover:bg-card border h-10 w-10 md:w-auto md:h-auto md:px-3 p-0 rounded-full md:rounded-lg"
            onClick={() => handleAIFeature("neighborhoodInsights")}
            title="Area Insights"
          >
            <MapPin className="h-4 w-4 md:mr-2 text-primary" />
            <span className="hidden md:inline">Insights</span>
          </Button>
          <Button
            size="sm"
            variant="secondary"
            className="shadow-lg bg-card/95 backdrop-blur-sm hover:bg-card border h-10 w-10 md:w-auto md:h-auto md:px-3 p-0 rounded-full md:rounded-lg"
            onClick={() => handleAIFeature("investmentTips")}
            title="Investment Tips"
          >
            <Lightbulb className="h-4 w-4 md:mr-2 text-primary" />
            <span className="hidden md:inline">Tips</span>
          </Button>
          <Button
            size="sm"
            variant="secondary"
            className="shadow-lg bg-card/95 backdrop-blur-sm hover:bg-card border h-10 w-10 md:w-auto md:h-auto md:px-3 p-0 rounded-full md:rounded-lg"
            onClick={() => handleAIFeature("priceEstimate")}
            title="AI Price Estimate"
          >
            <Sparkles className="h-4 w-4 md:mr-2 text-primary" />
            <span className="hidden md:inline">AI Price</span>
          </Button>
        </div>
      </div>

      {showAIPanel && (
        <div className="absolute bottom-4 left-4 right-4 md:left-auto md:right-4 md:w-96 z-20 ai-insights-panel rounded-xl shadow-xl border overflow-hidden">
          <div className="p-4 border-b bg-primary/5 flex items-center justify-between">
            <div className="flex items-center gap-2">
              <div className="p-2 rounded-full bg-primary/10">
                {activeAIFeature && getAIFeatureIcon(activeAIFeature)}
              </div>
              <div>
                <h3 className="font-semibold text-sm">{activeAIFeature && getAIFeatureTitle(activeAIFeature)}</h3>
                <p className="text-xs text-muted-foreground">Powered by KosHome AI</p>
              </div>
            </div>
            <Button
              size="icon"
              variant="ghost"
              className="h-8 w-8"
              onClick={() => {
                setShowAIPanel(false)
                setAiInsight(null)
              }}
            >
              <X className="h-4 w-4" />
            </Button>
          </div>
          <div className="p-4 max-h-64 overflow-y-auto">
            {aiLoading ? (
              <div className="flex flex-col items-center justify-center py-8">
                <div className="relative">
                  <Sparkles className="h-8 w-8 text-primary ai-pulse" />
                </div>
                <p className="text-sm text-muted-foreground mt-3">Analyzing data</p>
                <div className="flex gap-1 mt-2">
                  <span className="w-2 h-2 bg-primary rounded-full typing-dot"></span>
                  <span className="w-2 h-2 bg-primary rounded-full typing-dot"></span>
                  <span className="w-2 h-2 bg-primary rounded-full typing-dot"></span>
                </div>
              </div>
            ) : aiInsight ? (
              <div className="space-y-3">
                {aiInsight.content.map((insight, index) => (
                  <div key={index} className="p-3 rounded-lg bg-muted/50 text-sm leading-relaxed">
                    {insight}
                  </div>
                ))}
              </div>
            ) : null}
          </div>
        </div>
      )}
    </div>
  )
}
