"use client"

import type React from "react"
import { useState } from "react"
import Link from "next/link"
import { useRouter } from "next/navigation"
import type { Apartment } from "@/lib/types"
import { Button } from "@/components/ui/button"
import { Badge } from "@/components/ui/badge"
import { Heart, ChevronLeft, ChevronRight, Star, MapPin, Bed, Bath, Square } from "lucide-react"
import { cn } from "@/lib/utils"

interface ApartmentCardProps {
  apartment: Apartment
}

export function ApartmentCard({ apartment }: ApartmentCardProps) {
  const [currentImageIndex, setCurrentImageIndex] = useState(0)
  const [isHovered, setIsHovered] = useState(false)
  const [isFavorite, setIsFavorite] = useState(false)
  const router = useRouter()
  const location = apartment.location ?? {
    address: apartment.address || "",
    city: "Unknown",
    district: "Unknown",
    country: "Kosovo",
    coordinates: [apartment.latitude ?? 42.6629, apartment.longitude ?? 21.1655] as [number, number],
  }
  const title = apartment.title || "Apartment"
  const images = apartment.images ?? []
  const galleryImages = images.length > 0 ? images : ["/placeholder.svg"]
  const area = apartment.area ?? apartment.squareMeters ?? 0
  const listingType = apartment.listingType?.toString().toLowerCase()
  const isForSale = apartment.forSale ?? listingType === "sale"
  const isForRent = apartment.forRent ?? listingType === "rent"
  const listingLabel = isForSale
    ? "FOR SALE"
    : isForRent
      ? "FOR RENT"
      : apartment.listingType
        ? apartment.listingType.toString().toUpperCase()
        : "FOR SALE"

  const handlePrevImage = (e: React.MouseEvent) => {
    e.preventDefault()
    e.stopPropagation()
    setCurrentImageIndex((prev) => (prev === 0 ? galleryImages.length - 1 : prev - 1))
  }

  const handleNextImage = (e: React.MouseEvent) => {
    e.preventDefault()
    e.stopPropagation()
    setCurrentImageIndex((prev) => (prev === galleryImages.length - 1 ? 0 : prev + 1))
  }

  const handleFavoriteToggle = (e: React.MouseEvent) => {
    e.preventDefault()
    e.stopPropagation()
    setIsFavorite(!isFavorite)
  }

  const navigateToDetail = () => {
    router.push(`/apartments/${apartment.id}`)
  }

  return (
    <div
      className="group rounded-xl overflow-hidden border border-border bg-card transition-all duration-200 hover:shadow-md"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      {/* Image Gallery */}
      <div className="relative aspect-square overflow-hidden">
        <Link href={`/apartments/${apartment.id}`} className="block">
          <img
            src={galleryImages[currentImageIndex] || "/placeholder.svg"}
            alt={title}
            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        </Link>

        {/* Navigation Arrows - Only show on hover */}
        {isHovered && (
          <>
            <Button
              variant="secondary"
              size="icon"
              className="absolute left-2 top-1/2 -translate-y-1/2 rounded-full opacity-90 hover:opacity-100 h-8 w-8"
              onClick={handlePrevImage}
            >
              <ChevronLeft className="h-5 w-5" />
              <span className="sr-only">Previous image</span>
            </Button>

            <Button
              variant="secondary"
              size="icon"
              className="absolute right-2 top-1/2 -translate-y-1/2 rounded-full opacity-90 hover:opacity-100 h-8 w-8"
              onClick={handleNextImage}
            >
              <ChevronRight className="h-5 w-5" />
              <span className="sr-only">Next image</span>
            </Button>
          </>
        )}

        {/* Favorite Button */}
        <Button
          variant="secondary"
          size="icon"
          className={cn(
            "absolute top-2 right-2 rounded-full h-8 w-8",
            isFavorite ? "text-red-500" : "text-foreground opacity-90 hover:opacity-100",
          )}
          onClick={handleFavoriteToggle}
        >
          <Heart className={cn("h-5 w-5", isFavorite ? "fill-current" : "")} />
          <span className="sr-only">{isFavorite ? "Remove from favorites" : "Add to favorites"}</span>
        </Button>

        {/* Image Pagination Dots */}
        <div className="absolute bottom-2 left-0 right-0 flex justify-center gap-1">
          {galleryImages.map((_, index) => (
            <span
              key={index}
              className={cn(
                "h-1.5 w-1.5 rounded-full transition-all",
                currentImageIndex === index ? "bg-white w-3" : "bg-white/60",
              )}
            />
          ))}
        </div>

        {/* Property Type Badge */}
        <Badge variant={isForSale ? "default" : "secondary"} className="absolute bottom-2 left-2">
          {listingLabel}
        </Badge>
      </div>

      {/* Content */}
      <div className="p-4 space-y-3">
        <div className="flex justify-between items-start">
          <div>
            <Link href={`/apartments/${apartment.id}`} className="block">
              <h3 className="font-semibold text-lg line-clamp-1 hover:text-primary">{title}</h3>
            </Link>
            <div className="flex items-center text-sm text-muted-foreground">
              <MapPin className="h-3 w-3 mr-1" />
              <span>
                {location.district}, {location.city}
              </span>
            </div>
          </div>

          <div className="flex items-center">
            <Star className="h-4 w-4 text-primary fill-current mr-1" />
            <span className="font-medium">4.9</span>
          </div>
        </div>

        <div className="flex items-center gap-4 text-sm">
          <div className="flex items-center">
            <Bed className="h-4 w-4 mr-1 text-muted-foreground" />
            <span>{apartment.bedrooms}</span>
          </div>
          <div className="flex items-center">
            <Bath className="h-4 w-4 mr-1 text-muted-foreground" />
            <span>{apartment.bathrooms}</span>
          </div>
          <div className="flex items-center">
            <Square className="h-4 w-4 mr-1 text-muted-foreground" />
            <span>{area} m²</span>
          </div>
        </div>

        <div className="pt-2 border-t">
          <div className="flex items-end justify-between">
            <div>
              <span className="text-xl font-bold text-primary">{apartment.price.toLocaleString()} €</span>
              {!apartment.forSale && <span className="text-sm text-muted-foreground ml-1">/month</span>}
            </div>
            <Link href={`/apartments/${apartment.id}`} className="text-sm text-primary hover:underline">
              View details
            </Link>
          </div>
        </div>
      </div>
    </div>
  )
}
