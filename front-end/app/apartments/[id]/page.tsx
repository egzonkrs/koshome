"use client"

import { useState, useEffect } from "react"
import Link from "next/link"
import { useParams, useRouter } from "next/navigation"
import dynamic from "next/dynamic"
import { ArrowLeft, Heart, Share, Phone, Mail, MapPin, Bed, Bath, Square, Building, Check } from "lucide-react"
import { Header } from "@/components/header"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Separator } from "@/components/ui/separator"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { PropertyFeatureTag } from "@/components/property-feature-tag"
import { ImageZoomGallery } from "@/components/image-zoom-gallery"
import { useMobile } from "@/hooks/use-mobile"
import { Skeleton } from "@/components/ui/skeleton"
import { useApartment } from "@/lib/query"
import L from "leaflet"
import "leaflet/dist/leaflet.css"

const MapContainer = dynamic(() => import("react-leaflet").then((mod) => mod.MapContainer), { ssr: false })
const TileLayer = dynamic(() => import("react-leaflet").then((mod) => mod.TileLayer), { ssr: false })
const Marker = dynamic(() => import("react-leaflet").then((mod) => mod.Marker), { ssr: false })
const Popup = dynamic(() => import("react-leaflet").then((mod) => mod.Popup), { ssr: false })

export default function ApartmentDetailPage() {
  const params = useParams()
  const router = useRouter()
  const id = params.id as string
  const { data: apartment, isLoading } = useApartment(id)
  const [mounted, setMounted] = useState(false)
  const [isFavorite, setIsFavorite] = useState(false)
  const [mapReady, setMapReady] = useState(false)
  const isMobile = useMobile()

  useEffect(() => {
    setMounted(true)

    if (typeof window !== "undefined") {
      delete (L.Icon.Default.prototype as any)._getIconUrl
      L.Icon.Default.mergeOptions({
        iconRetinaUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon-2x.png",
        iconUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-icon.png",
        shadowUrl: "https://unpkg.com/leaflet@1.7.1/dist/images/marker-shadow.png",
      })
      setMapReady(true)
    }
  }, [])

  if (!mounted) {
    return null
  }

  if (isLoading) {
    return (
      <div className="min-h-screen flex flex-col">
        <Header />
        <div className="flex-1 flex items-center justify-center">
          <Skeleton className="h-64 w-full max-w-3xl" />
        </div>
      </div>
    )
  }

  if (!apartment) {
    return (
      <div className="min-h-screen flex flex-col">
        <Header />
        <div className="flex-1 flex items-center justify-center">
          <div className="text-center">
            <h1 className="text-2xl font-bold mb-4">Apartment not found</h1>
            <Link href="/">
              <Button>Go back to home</Button>
            </Link>
          </div>
        </div>
      </div>
    )
  }

  const location = apartment.location ?? {
    address: apartment.address || "",
    city: "Unknown",
    district: "Unknown",
    country: "Kosovo",
    coordinates: [apartment.latitude ?? 42.6629, apartment.longitude ?? 21.1655] as [number, number],
  }
  const title = apartment.title || "Apartment"
  const description = apartment.description || "No description available."
  const images = apartment.images ?? []
  const galleryImages = images.length > 0 ? images : ["/placeholder.svg"]
  const features = apartment.features ?? []
  const area = apartment.area ?? apartment.squareMeters ?? 0
  const pricePerSqm = apartment.pricePerSqm ?? (area > 0 ? Math.round(apartment.price / area) : undefined)
  const listingType = apartment.listingType?.toString().toLowerCase()
  const isForSale = apartment.forSale ?? listingType === "sale"
  const isForRent = apartment.forRent ?? listingType === "rent"
  const listingLabel = isForSale
    ? "SALE"
    : isForRent
      ? "RENT"
      : apartment.listingType
        ? apartment.listingType.toString().toUpperCase()
        : "SALE"

  const createPropertyMarkerIcon = () => {
    return L.divIcon({
      html: `<div class="flex items-center justify-center w-10 h-10 rounded-full bg-primary text-primary-foreground font-semibold text-xs shadow-lg">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>
      </div>`,
      className: "property-marker-icon",
      iconSize: L.point(40, 40),
      iconAnchor: [20, 40],
      popupAnchor: [0, -40],
    })
  }

  const handleFavoriteToggle = () => {
    setIsFavorite(!isFavorite)
  }

  const handleShare = () => {
    if (navigator.share) {
      navigator
        .share({
          title,
          text: `Check out this property: ${title}`,
          url: window.location.href,
        })
        .catch((error) => console.log("Error sharing", error))
    } else {
      // Fallback for browsers that don't support the Web Share API
      navigator.clipboard
        .writeText(window.location.href)
        .then(() => alert("Link copied to clipboard!"))
        .catch((error) => console.error("Could not copy text: ", error))
    }
  }

  return (
    <div className="min-h-screen flex flex-col bg-background">
      <Header />

      <main className="flex-1 container mx-auto px-4 py-6">
        {/* Back button */}
        <div className="mb-6">
          <Button variant="ghost" size="sm" onClick={() => router.back()} className="group">
            <ArrowLeft className="mr-2 h-4 w-4 transition-transform group-hover:-translate-x-1" />
            Back to listings
          </Button>
        </div>

        {/* Main content */}
        <div className="grid grid-cols-1 lg:grid-cols-[2fr_1fr] gap-8">
          {/* Left column - Images and details */}
          <div className="space-y-8">
            {/* Title and actions */}
            <div className="flex flex-col md:flex-row md:items-start justify-between gap-4">
              <div>
                <h1 className="text-3xl font-bold">{title}</h1>
                <div className="flex items-center mt-2 text-muted-foreground">
                  <MapPin className="h-4 w-4 mr-1 flex-shrink-0" />
                  <span className="truncate">
                    {location.address}, {location.district}, {location.city}
                  </span>
                </div>
              </div>

              <div className="flex items-center gap-2 self-end md:self-start">
                <Button
                  variant="outline"
                  size="icon"
                  className="rounded-full bg-transparent"
                  onClick={handleFavoriteToggle}
                >
                  <Heart className={`h-5 w-5 ${isFavorite ? "fill-red-500 text-red-500" : ""}`} />
                  <span className="sr-only">{isFavorite ? "Remove from favorites" : "Add to favorites"}</span>
                </Button>

                <Button variant="outline" size="icon" className="rounded-full bg-transparent" onClick={handleShare}>
                  <Share className="h-5 w-5" />
                  <span className="sr-only">Share</span>
                </Button>
              </div>
            </div>

            {/* Key specs */}
            <div className="flex flex-wrap items-center gap-6 md:gap-8">
              <div className="flex items-center gap-2">
                <div className="bg-primary/10 p-2 rounded-full">
                  <Bed className="h-5 w-5 text-primary" />
                </div>
                <div>
                  <p className="text-sm text-muted-foreground">Bedrooms</p>
                  <p className="font-semibold">{apartment.bedrooms}</p>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <div className="bg-primary/10 p-2 rounded-full">
                  <Bath className="h-5 w-5 text-primary" />
                </div>
                <div>
                  <p className="text-sm text-muted-foreground">Bathrooms</p>
                  <p className="font-semibold">{apartment.bathrooms}</p>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <div className="bg-primary/10 p-2 rounded-full">
                  <Square className="h-5 w-5 text-primary" />
                </div>
                <div>
                  <p className="text-sm text-muted-foreground">Area</p>
                  <p className="font-semibold">{area} m²</p>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <div className="bg-primary/10 p-2 rounded-full">
                  <Building className="h-5 w-5 text-primary" />
                </div>
                <div>
                  <p className="text-sm text-muted-foreground">Floor</p>
                  <p className="font-semibold">{apartment.floor ?? "N/A"}</p>
                </div>
              </div>
            </div>

            {/* Feature tags */}
            <div className="flex flex-wrap gap-2">
              <PropertyFeatureTag>
                <Badge variant={isForSale ? "default" : "secondary"} className="mr-1">
                  {listingLabel}
                </Badge>
                {isForSale ? "For Sale" : isForRent ? "For Rent" : "Listing"}
              </PropertyFeatureTag>

              {features.slice(0, 5).map((feature) => (
                <PropertyFeatureTag key={feature}>
                  <Check className="h-3.5 w-3.5 mr-1 inline-block text-primary" />
                  {feature}
                </PropertyFeatureTag>
              ))}
            </div>

            {/* Image gallery */}
            <ImageZoomGallery images={galleryImages} title={title} />

            {/* Tabs for details */}
            <Tabs defaultValue="description" className="mt-8">
              <TabsList className="grid w-full grid-cols-3">
                <TabsTrigger value="description">Description</TabsTrigger>
                <TabsTrigger value="features">Features</TabsTrigger>
                <TabsTrigger value="location">Location</TabsTrigger>
              </TabsList>

              <TabsContent value="description" className="mt-4">
                <div className="prose prose-sm max-w-none dark:prose-invert">
                  <p className="text-sm leading-relaxed text-muted-foreground">{description}</p>
                  <p className="text-sm leading-relaxed text-muted-foreground mt-3">
                    This property is located in the desirable {location.district} district, known for its
                    excellent amenities and convenient location. The apartment offers {apartment.bedrooms} bedroom(s),{" "}
                    {apartment.bathrooms} bathroom(s), and a total area of {area} m².
                  </p>
                  <p className="text-sm leading-relaxed text-muted-foreground mt-3">
                    Whether you're looking for a new home or an investment opportunity, this property offers excellent
                    value at {apartment.price.toLocaleString()} € ({pricePerSqm ? `${pricePerSqm} €/m²` : "N/A"}).
                  </p>
                </div>
              </TabsContent>

              <TabsContent value="features" className="mt-4">
                <div className="grid grid-cols-2 md:grid-cols-3 gap-4">
                  {features.map((feature) => (
                    <div key={feature} className="flex items-center gap-2">
                      <Check className="h-5 w-5 text-primary" />
                      <span>{feature}</span>
                    </div>
                  ))}
                </div>
              </TabsContent>

              <TabsContent value="location" className="mt-4">
                <div className="aspect-video rounded-lg overflow-hidden border relative">
                  {mapReady ? (
                    <MapContainer
                      center={location.coordinates}
                      zoom={16}
                      minZoom={10}
                      maxZoom={19}
                      style={{ height: "100%", width: "100%" }}
                      scrollWheelZoom={true}
                      touchZoom={true}
                      dragging={true}
                    >
                      <TileLayer
                        attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>'
                        url="https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png"
                        maxZoom={20}
                        maxNativeZoom={19}
                      />
                      <Marker position={location.coordinates} icon={createPropertyMarkerIcon()}>
                        <Popup>
                          <div className="p-2">
                            <h4 className="font-semibold text-sm">{title}</h4>
                            <p className="text-xs text-muted-foreground mt-1">{location.address}</p>
                            <p className="text-sm font-bold text-primary mt-2">{apartment.price.toLocaleString()} €</p>
                          </div>
                        </Popup>
                      </Marker>
                    </MapContainer>
                  ) : (
                    <Skeleton className="w-full h-full" />
                  )}
                </div>
                <div className="mt-4">
                  <h3 className="font-semibold mb-2">Address</h3>
                  <p className="text-sm text-muted-foreground">
                    {location.address}, {location.district}, {location.city}, {location.country}
                  </p>
                </div>
              </TabsContent>
            </Tabs>
          </div>

          {/* Right column - Price and contact */}
          <div>
            <div className="sticky top-24 space-y-6">
              {/* Price card */}
              <div className="bg-card rounded-lg border shadow-sm overflow-hidden">
                <div className="p-6">
                  <div className="flex items-end gap-2 mb-1">
                    <span className="text-3xl font-bold text-primary">{apartment.price.toLocaleString()} €</span>
                    {!apartment.forSale && <span className="text-muted-foreground">/month</span>}
                  </div>
                  <div className="text-sm text-muted-foreground mb-6">
                    {pricePerSqm ? `${pricePerSqm} €/m²` : "N/A"} · Listed on{" "}
                    {new Date(apartment.createdAt).toLocaleDateString()}
                  </div>

                  <div className="space-y-3">
                    <Button className="w-full" size="lg">
                      Contact Agent
                    </Button>
                    <Button variant="outline" className="w-full bg-transparent">
                      Schedule Viewing
                    </Button>
                  </div>
                </div>

                <Separator />

                <div className="p-6">
                  <h3 className="text-lg font-semibold mb-4">Financing</h3>
                  <div className="space-y-4">
                    <div className="flex justify-between items-center">
                      <span>Monthly payment</span>
                      <span className="font-semibold">€1,104/month</span>
                    </div>
                    <div className="flex justify-between items-center">
                      <span>Down payment (20%)</span>
                      <span className="font-semibold">€{Math.round(apartment.price * 0.2).toLocaleString()}</span>
                    </div>
                    <Button variant="outline" className="w-full bg-transparent">
                      Calculate Mortgage
                    </Button>
                  </div>
                </div>
              </div>

              {/* Agent card */}
              <div className="bg-card rounded-lg border shadow-sm overflow-hidden">
                <div className="p-6">
                  <h3 className="text-lg font-semibold mb-4">Contact Agent</h3>
                  <div className="flex items-center gap-4 mb-6">
                    <div className="w-16 h-16 rounded-full bg-muted flex items-center justify-center">
                      <span className="text-xl font-semibold">KT</span>
                    </div>
                    <div>
                      <div className="font-semibold">KosHome Tester</div>
                      <div className="text-sm text-muted-foreground">KosHome Agent</div>
                    </div>
                  </div>
                  <div className="space-y-4">
                    <Button className="w-full">
                      <Phone className="mr-2 h-4 w-4" />
                      Call Agent
                    </Button>
                    <Button variant="outline" className="w-full bg-transparent">
                      <Mail className="mr-2 h-4 w-4" />
                      Email Agent
                    </Button>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </main>
    </div>
  )
}
