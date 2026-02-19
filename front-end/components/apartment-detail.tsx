"use client"

import { useApartmentStore } from "@/store/apartment-store"
import { Button } from "@/components/ui/button"
import { X, Heart, Share, Phone, Mail } from "lucide-react"
import { Badge } from "@/components/ui/badge"
import { Separator } from "@/components/ui/separator"

export function ApartmentDetail() {
  const { selectedApartment, setSelectedApartment } = useApartmentStore()

  if (!selectedApartment) return null
  const location = selectedApartment.location ?? {
    address: selectedApartment.address || "",
    city: "Unknown",
    district: "Unknown",
    country: "Kosovo",
    coordinates: [selectedApartment.latitude ?? 42.6629, selectedApartment.longitude ?? 21.1655] as [number, number],
  }
  const title = selectedApartment.title || "Apartment"
  const images = selectedApartment.images ?? []
  const features = selectedApartment.features ?? []
  const area = selectedApartment.area ?? selectedApartment.squareMeters ?? 0
  const pricePerSqm =
    selectedApartment.pricePerSqm ?? (area > 0 ? Math.round(selectedApartment.price / area) : undefined)
  const listingType = selectedApartment.listingType?.toString().toLowerCase()
  const isForSale = selectedApartment.forSale ?? listingType === "sale"
  const isForRent = selectedApartment.forRent ?? listingType === "rent"
  const listingLabel = isForSale
    ? "FOR SALE"
    : isForRent
      ? "FOR RENT"
      : selectedApartment.listingType
        ? selectedApartment.listingType.toString().toUpperCase()
        : "FOR SALE"

  return (
    <div className="fixed inset-0 z-50 bg-background/80 backdrop-blur-sm md:hidden">
      <div className="fixed inset-x-0 bottom-0 top-16 z-50 h-[calc(100vh-4rem)] w-full overflow-y-auto rounded-t-xl bg-background p-6 shadow-lg animate-in slide-in-from-bottom-10">
        <div className="absolute right-4 top-4">
          <Button variant="ghost" size="icon" onClick={() => setSelectedApartment(null)}>
            <X className="h-4 w-4" />
            <span className="sr-only">Close</span>
          </Button>
        </div>

        <div className="relative h-64 -mx-6 -mt-6 mb-6">
          <img
            src={images[0] || "/placeholder.svg"}
            alt={title}
            className="w-full h-full object-cover"
          />
          <div className="absolute bottom-4 right-4 flex gap-2">
            <Button size="icon" variant="secondary" className="rounded-full">
              <Heart className="h-4 w-4" />
              <span className="sr-only">Add to favorites</span>
            </Button>
            <Button size="icon" variant="secondary" className="rounded-full">
              <Share className="h-4 w-4" />
              <span className="sr-only">Share</span>
            </Button>
          </div>
        </div>

        <div className="space-y-6">
          <div>
            <div className="flex items-center justify-between">
              <h2 className="text-2xl font-semibold">{selectedApartment.price.toLocaleString()} €</h2>
              <Badge variant={isForSale ? "default" : "secondary"}>
                {listingLabel}
              </Badge>
            </div>
            <p className="text-muted-foreground">{pricePerSqm ? `${pricePerSqm} €/m²` : "N/A"}</p>
          </div>

          <div>
            <h1 className="text-xl font-semibold">{title}</h1>
            <p className="text-muted-foreground">
              {location.address}, {location.district}, {location.city}
            </p>
          </div>

          <div className="flex items-center justify-between">
            <div className="flex items-center gap-4">
              <div className="text-center">
                <p className="font-semibold">{selectedApartment.bedrooms}</p>
                <p className="text-xs text-muted-foreground">Bedrooms</p>
              </div>
              <div className="text-center">
                <p className="font-semibold">{selectedApartment.bathrooms}</p>
                <p className="text-xs text-muted-foreground">Bathrooms</p>
              </div>
              <div className="text-center">
                <p className="font-semibold">{area}</p>
                <p className="text-xs text-muted-foreground">m²</p>
              </div>
              <div className="text-center">
                <p className="font-semibold">{selectedApartment.floor ?? "N/A"}</p>
                <p className="text-xs text-muted-foreground">Floor</p>
              </div>
            </div>
          </div>

          <Separator />

          <div>
            <h3 className="font-semibold mb-2">Description</h3>
            <p className="text-sm text-muted-foreground">{selectedApartment.description}</p>
          </div>

          <div>
            <h3 className="font-semibold mb-2">Features</h3>
            <div className="flex flex-wrap gap-2">
              {features.map((feature) => (
                <Badge key={feature} variant="outline">
                  {feature}
                </Badge>
              ))}
            </div>
          </div>

          <Separator />

          <div>
            <h3 className="font-semibold mb-4">Contact Agent</h3>
            <div className="flex gap-4">
              <Button className="flex-1">
                <Phone className="mr-2 h-4 w-4" />
                Call
              </Button>
              <Button variant="outline" className="flex-1">
                <Mail className="mr-2 h-4 w-4" />
                Email
              </Button>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
