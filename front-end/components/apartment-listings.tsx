"use client"

import { useState } from "react"
import { useRouter } from "next/navigation"
import { useApartmentStore } from "@/store/apartment-store"
import { Heart, ArrowRight, Bed, Bath, Square } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Skeleton } from "@/components/ui/skeleton"
import { cn } from "@/lib/utils"
import Link from "next/link"

export function ApartmentListings() {
  const { filteredApartments, setSelectedApartment } = useApartmentStore()
  const router = useRouter()
  const [hoveredId, setHoveredId] = useState<string | null>(null)

  const handleViewDetails = (id: string) => {
    router.push(`/apartments/${id}`)
  }

  const handleMouseEnter = (id: string) => {
    setHoveredId(id)
    const apartment = filteredApartments.find((apt) => apt.id === id)
    if (apartment) {
      setSelectedApartment(apartment)
    }
  }

  const handleMouseLeave = () => {
    setHoveredId(null)
    setSelectedApartment(null)
  }

  if (filteredApartments.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center h-full p-8 text-center">
        <div className="text-muted-foreground mb-4">No properties match your search criteria.</div>
        <Button variant="outline" onClick={() => useApartmentStore.getState().resetFilters()}>
          Reset Filters
        </Button>
      </div>
    )
  }

  return (
    <div className="divide-y">
      {filteredApartments.slice(0, 5).map((apartment) => {
        const location = apartment.location ?? {
          address: apartment.address || "",
          city: "Unknown",
          district: "Unknown",
          country: "Kosovo",
          coordinates: [apartment.latitude ?? 42.6629, apartment.longitude ?? 21.1655] as [number, number],
        }
        const title = apartment.title || "Apartment"
        const images = apartment.images ?? []
        const area = apartment.area ?? apartment.squareMeters ?? 0
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

        return (
          <div
            key={apartment.id}
            className={cn("p-4 transition-colors", hoveredId === apartment.id ? "bg-muted/50" : "hover:bg-muted/30")}
            onMouseEnter={() => handleMouseEnter(apartment.id)}
            onMouseLeave={handleMouseLeave}
          >
            <div className="flex gap-4">
              <Link
                href={`/apartments/${apartment.id}`}
                className="relative w-24 h-24 sm:w-32 sm:h-32 flex-shrink-0 rounded-md overflow-hidden"
              >
                <img src={images[0] || "/placeholder.svg"} alt={title} className="w-full h-full object-cover" />
                <div className="absolute top-2 right-2">
                  <Button
                    size="icon"
                    variant="ghost"
                    className="h-7 w-7 rounded-full bg-white/80 text-foreground hover:bg-white/90"
                  >
                    <Heart className="h-4 w-4" />
                    <span className="sr-only">Add to favorites</span>
                  </Button>
                </div>
                <Badge className="absolute bottom-2 left-2" variant={isForSale ? "default" : "secondary"}>
                  {listingLabel}
                </Badge>
              </Link>

              <div className="flex-1 min-w-0 flex flex-col justify-between py-0.5">
                <div>
                  <Link href={`/apartments/${apartment.id}`} className="block">
                    <h3 className="font-semibold text-sm truncate hover:text-primary">{title}</h3>
                  </Link>

                  <p className="text-xs text-muted-foreground truncate mt-0.5">
                    {location.district}, {location.city}
                  </p>

                  <div className="flex items-center gap-3 mt-1 text-xs">
                    <div className="flex items-center gap-1">
                      <Bed className="h-3 w-3 text-muted-foreground" />
                      <span>{apartment.bedrooms}</span>
                    </div>
                    <div className="flex items-center gap-1">
                      <Bath className="h-3 w-3 text-muted-foreground" />
                      <span>{apartment.bathrooms}</span>
                    </div>
                    <div className="flex items-center gap-1">
                      <Square className="h-3 w-3 text-muted-foreground" />
                      <span>{area} m²</span>
                    </div>
                  </div>

                  <p className="text-xs text-muted-foreground line-clamp-1 mt-1">{apartment.description}</p>
                </div>

                <div className="mt-1 flex justify-between items-center">
                  <span className="font-bold text-sm text-primary">{apartment.price.toLocaleString()} €</span>
                  <Button
                    size="sm"
                    variant="outline"
                    className="h-7 text-xs px-2"
                    onClick={() => handleViewDetails(apartment.id)}
                  >
                    View Details
                    <ArrowRight className="ml-1 h-3 w-3" />
                  </Button>
                </div>
              </div>
            </div>
          </div>
        )
      })}

      {filteredApartments.length > 5 && (
        <div className="p-4 text-center">
          <Button variant="link" onClick={() => router.push("/apartments")}>
            View All {filteredApartments.length} Properties
          </Button>
        </div>
      )}
    </div>
  )
}

export function ApartmentListingsSkeleton() {
  return (
    <div className="divide-y">
      {Array.from({ length: 4 }).map((_, i) => (
        <div key={i} className="p-4">
          <div className="flex gap-4">
            <Skeleton className="w-32 h-32 rounded-md" />
            <div className="flex-1 space-y-2">
              <div className="flex justify-between">
                <Skeleton className="h-5 w-3/5" />
                <Skeleton className="h-5 w-1/5" />
              </div>
              <Skeleton className="h-4 w-4/5" />
              <Skeleton className="h-4 w-2/5" />
              <Skeleton className="h-4 w-full" />
              <Skeleton className="h-4 w-full" />
              <div className="flex justify-end">
                <Skeleton className="h-8 w-24" />
              </div>
            </div>
          </div>
        </div>
      ))}
    </div>
  )
}
