"use client"

import Link from "next/link"
import type { Apartment } from "@/lib/types"
import { Heart } from "lucide-react"
import { Button } from "@/components/ui/button"
import { useApartmentStore } from "@/store/apartment-store"

interface ApartmentCardProps {
  apartment: Apartment
}

export function ApartmentCard({ apartment }: ApartmentCardProps) {
  const { setSelectedApartment, selectedApartment } = useApartmentStore()
  const isSelected = selectedApartment?.id === apartment.id
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
  const pricePerSqm = apartment.pricePerSqm ?? (area > 0 ? Math.round(apartment.price / area) : undefined)
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

  const handleCardClick = () => {
    setSelectedApartment(apartment)
  }

  return (
    <div
      className={`border rounded-lg overflow-hidden transition-all ${
        isSelected ? "ring-2 ring-primary" : "hover:shadow-md"
      }`}
      onClick={handleCardClick}
    >
      <div className="relative">
        <Link href={`/apartments/${apartment.id}`} className="block">
          <img
            src={images[0] || "/placeholder.svg"}
            alt={title}
            className="h-48 w-full object-cover"
          />
        </Link>
        <div className="absolute top-2 right-2">
          <Button
            size="icon"
            variant="ghost"
            className="h-8 w-8 rounded-full bg-[#eeefe9]/80 text-foreground hover:bg-[#eeefe9]/90"
          >
            <Heart className="h-4 w-4" />
            <span className="sr-only">Add to favorites</span>
          </Button>
        </div>
        <div className="absolute bottom-0 left-0 bg-primary text-primary-foreground px-2 py-1 text-sm font-semibold">
          {listingLabel}
        </div>
      </div>

      <div className="p-4">
        <Link href={`/apartments/${apartment.id}`} className="hover:text-primary">
          <h3 className="font-semibold text-lg truncate">{title}</h3>
        </Link>
        <p className="text-sm text-muted-foreground">
          {location.district}, {location.city}
        </p>

        <div className="mt-2 flex items-center justify-between">
          <span className="text-xl font-bold">{apartment.price.toLocaleString()} €</span>
          <span className="text-sm text-muted-foreground">{pricePerSqm ? `${pricePerSqm} €/m²` : "N/A"}</span>
        </div>

        <div className="mt-3 flex items-center gap-4 text-sm">
          <div className="flex items-center gap-1">
            <span className="font-medium">{apartment.bedrooms}</span>
            <span className="text-muted-foreground">BD</span>
          </div>
          <div className="flex items-center gap-1">
            <span className="font-medium">{apartment.bathrooms}</span>
            <span className="text-muted-foreground">BA</span>
          </div>
          <div className="flex items-center gap-1">
            <span className="font-medium">{area}</span>
            <span className="text-muted-foreground">m²</span>
          </div>
        </div>

        <p className="mt-3 text-sm text-muted-foreground line-clamp-2">{apartment.description}</p>
      </div>
    </div>
  )
}
