"use client"

import { useState, useEffect } from "react"
import { useForm } from "react-hook-form"
import { Loader2, Sparkles, TrendingUp, X } from "lucide-react"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Textarea } from "@/components/ui/textarea"
import { Switch } from "@/components/ui/switch"
import { Badge } from "@/components/ui/badge"
import { Sheet, SheetContent, SheetDescription, SheetHeader, SheetTitle } from "@/components/ui/sheet"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { useToast } from "@/hooks/use-toast"
import { useUserStore } from "@/store/user-store"
import { useLandlordStore, type LandlordListing, type ListingStatus } from "@/store/landlord-store"
import { cn } from "@/lib/utils"

const AVAILABLE_AMENITIES = [
  "Furnished",
  "Air Conditioning",
  "Internet",
  "Balcony",
  "Parking",
  "Elevator",
  "Security",
  "Garden",
  "Storage",
  "Pet Friendly",
  "Laundry",
  "Dishwasher",
  "Smart Home",
  "Gym Access",
  "Pool Access",
]

const DISTRICTS = [
  "City Center",
  "Arbëri",
  "Dardania",
  "Kalabria",
  "Old Town",
  "Ulpiana",
  "Veternik",
  "Bregu i Diellit",
]

interface ListingFormData {
  title: string
  description: string
  price: number
  area: number
  bedrooms: number
  bathrooms: number
  floor: number
  address: string
  district: string
  forSale: boolean
  forRent: boolean
  status: ListingStatus
}

interface ListingFormDrawerProps {
  open: boolean
  onOpenChange: (open: boolean) => void
  editListing?: LandlordListing | null
}

const AI_GENERATED_DESCRIPTIONS = [
  "Stunning and meticulously maintained apartment in the heart of Pristina. This exceptional property features sun-drenched living spaces, modern finishes throughout, and breathtaking city views. The open-concept layout seamlessly connects the living, dining, and kitchen areas, perfect for entertaining. Recent upgrades include high-end appliances, designer lighting, and premium flooring. Located in a prime location with easy access to shops, restaurants, and public transportation. Don't miss this rare opportunity to own a piece of urban luxury!",
  "Welcome to your new home! This beautifully renovated apartment offers the perfect blend of comfort and style. The spacious layout features large windows that flood the space with natural light, creating a warm and inviting atmosphere. The modern kitchen boasts granite countertops, stainless steel appliances, and ample cabinet space. Additional highlights include hardwood floors, updated bathrooms, and generous closet space. Situated in a quiet, tree-lined neighborhood with excellent schools nearby. Schedule your viewing today!",
  "Discover refined living in this exquisite apartment that combines contemporary design with everyday functionality. Every detail has been thoughtfully considered, from the chef's kitchen with premium finishes to the spa-like bathroom retreat. The versatile floor plan adapts to your lifestyle, whether you're working from home or hosting guests. Building amenities include secure entry, on-site management, and convenient parking. Located steps from cafes, parks, and cultural attractions. Your dream home awaits!",
]

export function ListingFormDrawer({ open, onOpenChange, editListing }: ListingFormDrawerProps) {
  const { toast } = useToast()
  const { user, useCredit } = useUserStore()
  const { addListing, updateListing } = useLandlordStore()

  const [selectedAmenities, setSelectedAmenities] = useState<string[]>([])
  const [isGeneratingDescription, setIsGeneratingDescription] = useState(false)
  const [isCheckingPrice, setIsCheckingPrice] = useState(false)

  const {
    register,
    handleSubmit,
    setValue,
    watch,
    reset,
    formState: { errors },
  } = useForm<ListingFormData>({
    defaultValues: {
      title: "",
      description: "",
      price: 0,
      area: 0,
      bedrooms: 1,
      bathrooms: 1,
      floor: 0,
      address: "",
      district: "",
      forSale: false,
      forRent: true,
      status: "pending",
    },
  })

  const description = watch("description")
  const forSale = watch("forSale")
  const forRent = watch("forRent")

  // Reset form when opening/closing or when editListing changes
  useEffect(() => {
    if (open && editListing) {
      const location = editListing.location ?? { address: "", district: "" }
      reset({
        title: editListing.title || "",
        description: editListing.description || "",
        price: editListing.price,
        area: editListing.area ?? editListing.squareMeters ?? 0,
        bedrooms: editListing.bedrooms,
        bathrooms: editListing.bathrooms,
        floor: editListing.floor ?? 0,
        address: location.address,
        district: location.district,
        forSale: editListing.forSale ?? false,
        forRent: editListing.forRent ?? true,
        status: editListing.status,
      })
      setSelectedAmenities(editListing.features ?? [])
    } else if (open && !editListing) {
      reset({
        title: "",
        description: "",
        price: 0,
        area: 0,
        bedrooms: 1,
        bathrooms: 1,
        floor: 0,
        address: "",
        district: "",
        forSale: false,
        forRent: true,
        status: "pending",
      })
      setSelectedAmenities([])
    }
  }, [open, editListing, reset])

  const handleGenerateDescription = async () => {
    if (!user) {
      toast({
        title: "Not logged in",
        description: "Please log in to use AI features.",
        variant: "destructive",
      })
      return
    }

    if (user.plan === "free") {
      toast({
        title: "Upgrade Required",
        description: "AI features are not available on the Free plan. Please upgrade to Standard or Premium.",
        variant: "destructive",
      })
      return
    }

    if (user.credits <= 0 && user.plan !== "premium") {
      toast({
        title: "No Credits Left",
        description: "You've used all your AI credits. Please upgrade your plan for more.",
        variant: "destructive",
      })
      return
    }

    setIsGeneratingDescription(true)

    // Simulate AI generation delay
    await new Promise((resolve) => setTimeout(resolve, 2000))

    // Pick a random description
    const randomDescription = AI_GENERATED_DESCRIPTIONS[Math.floor(Math.random() * AI_GENERATED_DESCRIPTIONS.length)]
    setValue("description", randomDescription)

    setIsGeneratingDescription(false)

    toast({
      title: "Description Generated!",
      description: "AI has created a compelling listing description for you.",
    })
  }

  const handleCheckMarketValue = async () => {
    if (!user) {
      toast({
        title: "Not logged in",
        description: "Please log in to use AI features.",
        variant: "destructive",
      })
      return
    }

    if (user.plan === "free") {
      toast({
        title: "Upgrade Required",
        description: "AI features are not available on the Free plan. Please upgrade to Standard or Premium.",
        variant: "destructive",
      })
      return
    }

    if (user.credits <= 0 && user.plan !== "premium") {
      toast({
        title: "No Credits Left",
        description: "You've used all your AI credits. Please upgrade your plan for more.",
        variant: "destructive",
      })
      return
    }

    setIsCheckingPrice(true)

    // Simulate AI check delay
    await new Promise((resolve) => setTimeout(resolve, 1500))

    setIsCheckingPrice(false)

    // Generate a mock price range based on the entered price
    const currentPrice = watch("price") || 400
    const minPrice = Math.round(currentPrice * 0.9)
    const maxPrice = Math.round(currentPrice * 1.15)

    toast({
      title: "Market Value Analysis",
      description: `Estimated Range: €${minPrice.toLocaleString()} - €${maxPrice.toLocaleString()}`,
    })
  }

  const toggleAmenity = (amenity: string) => {
    setSelectedAmenities((prev) => (prev.includes(amenity) ? prev.filter((a) => a !== amenity) : [...prev, amenity]))
  }

  const onSubmit = (data: ListingFormData) => {
    if (!user) {
      toast({
        title: "Not logged in",
        description: "Please log in to manage listings.",
        variant: "destructive",
      })
      return
    }

    const listingData = {
      title: data.title,
      description: data.description,
      price: data.price,
      pricePerSqm: data.area > 0 ? Math.round(data.price / data.area) : 0,
      area: data.area,
      bedrooms: data.bedrooms,
      bathrooms: data.bathrooms,
      floor: data.floor,
      location: {
        address: data.address,
        city: "Pristina",
        district: data.district,
        country: "Kosovo",
        coordinates: [42.6629, 21.1655] as [number, number],
      },
      features: selectedAmenities,
      images: [
        "https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?ixlib=rb-4.0.3&auto=format&fit=crop&w=2340&q=80",
      ],
      forSale: data.forSale,
      forRent: data.forRent,
      status: data.status,
      ownerId: user.id,
    }

    if (editListing) {
      updateListing(editListing.id, listingData)
      toast({
        title: "Listing Updated",
        description: "Your listing has been successfully updated.",
      })
    } else {
      addListing(listingData)
      toast({
        title: "Listing Created",
        description: "Your new listing has been successfully created.",
      })
    }

    onOpenChange(false)
  }

  return (
    <Sheet open={open} onOpenChange={onOpenChange}>
      <SheetContent className="w-full sm:max-w-xl overflow-y-auto">
        <SheetHeader>
          <SheetTitle>{editListing ? "Edit Listing" : "Post New Listing"}</SheetTitle>
          <SheetDescription>
            {editListing
              ? "Update the details of your property listing."
              : "Fill in the details to create a new property listing."}
          </SheetDescription>
        </SheetHeader>

        <form onSubmit={handleSubmit(onSubmit)} className="space-y-6 mt-6">
          {/* Title */}
          <div className="space-y-2">
            <Label htmlFor="title">Title *</Label>
            <Input
              id="title"
              placeholder="e.g., Modern 2-Bedroom Apartment"
              {...register("title", { required: "Title is required" })}
            />
            {errors.title && <p className="text-sm text-destructive">{errors.title.message}</p>}
          </div>

          {/* Description with AI Generate */}
          <div className="space-y-2">
            <div className="flex items-center justify-between">
              <Label htmlFor="description">Description *</Label>
              <Button
                type="button"
                variant="outline"
                size="sm"
                onClick={handleGenerateDescription}
                disabled={isGeneratingDescription}
                className="gap-2 bg-transparent"
              >
                {isGeneratingDescription ? (
                  <>
                    <Loader2 className="h-4 w-4 animate-spin" />
                    Generating...
                  </>
                ) : (
                  <>
                    <Sparkles className="h-4 w-4" />
                    Generate with AI
                  </>
                )}
              </Button>
            </div>
            <Textarea
              id="description"
              placeholder="Describe your property..."
              className="min-h-[120px]"
              {...register("description", { required: "Description is required" })}
            />
            {errors.description && <p className="text-sm text-destructive">{errors.description.message}</p>}
          </div>

          {/* Price with Market Value Check */}
          <div className="space-y-2">
            <div className="flex items-center justify-between">
              <Label htmlFor="price">Price (€) *</Label>
              <Button
                type="button"
                variant="outline"
                size="sm"
                onClick={handleCheckMarketValue}
                disabled={isCheckingPrice}
                className="gap-2 bg-transparent"
              >
                {isCheckingPrice ? (
                  <>
                    <Loader2 className="h-4 w-4 animate-spin" />
                    Checking...
                  </>
                ) : (
                  <>
                    <TrendingUp className="h-4 w-4" />
                    Check Market Value
                  </>
                )}
              </Button>
            </div>
            <Input
              id="price"
              type="number"
              placeholder="e.g., 450"
              {...register("price", {
                required: "Price is required",
                valueAsNumber: true,
                min: { value: 1, message: "Price must be greater than 0" },
              })}
            />
            {errors.price && <p className="text-sm text-destructive">{errors.price.message}</p>}
          </div>

          {/* Area */}
          <div className="space-y-2">
            <Label htmlFor="area">Area (m²) *</Label>
            <Input
              id="area"
              type="number"
              placeholder="e.g., 80"
              {...register("area", {
                required: "Area is required",
                valueAsNumber: true,
                min: { value: 1, message: "Area must be greater than 0" },
              })}
            />
            {errors.area && <p className="text-sm text-destructive">{errors.area.message}</p>}
          </div>

          {/* Bedrooms & Bathrooms */}
          <div className="grid grid-cols-2 gap-4">
            <div className="space-y-2">
              <Label htmlFor="bedrooms">Bedrooms</Label>
              <Select
                value={watch("bedrooms")?.toString()}
                onValueChange={(value) => setValue("bedrooms", Number.parseInt(value))}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Select" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="0">Studio</SelectItem>
                  <SelectItem value="1">1</SelectItem>
                  <SelectItem value="2">2</SelectItem>
                  <SelectItem value="3">3</SelectItem>
                  <SelectItem value="4">4</SelectItem>
                  <SelectItem value="5">5+</SelectItem>
                </SelectContent>
              </Select>
            </div>
            <div className="space-y-2">
              <Label htmlFor="bathrooms">Bathrooms</Label>
              <Select
                value={watch("bathrooms")?.toString()}
                onValueChange={(value) => setValue("bathrooms", Number.parseInt(value))}
              >
                <SelectTrigger>
                  <SelectValue placeholder="Select" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="1">1</SelectItem>
                  <SelectItem value="2">2</SelectItem>
                  <SelectItem value="3">3</SelectItem>
                  <SelectItem value="4">4+</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          {/* Floor */}
          <div className="space-y-2">
            <Label htmlFor="floor">Floor</Label>
            <Input id="floor" type="number" placeholder="e.g., 3" {...register("floor", { valueAsNumber: true })} />
          </div>

          {/* Location */}
          <div className="space-y-2">
            <Label htmlFor="address">Address *</Label>
            <Input
              id="address"
              placeholder="e.g., 123 Main Street"
              {...register("address", { required: "Address is required" })}
            />
            {errors.address && <p className="text-sm text-destructive">{errors.address.message}</p>}
          </div>

          <div className="space-y-2">
            <Label htmlFor="district">District *</Label>
            <Select value={watch("district")} onValueChange={(value) => setValue("district", value)}>
              <SelectTrigger>
                <SelectValue placeholder="Select district" />
              </SelectTrigger>
              <SelectContent>
                {DISTRICTS.map((district) => (
                  <SelectItem key={district} value={district}>
                    {district}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>
          </div>

          {/* Listing Type */}
          <div className="space-y-4">
            <Label>Listing Type</Label>
            <div className="flex items-center gap-6">
              <div className="flex items-center gap-2">
                <Switch id="forRent" checked={forRent} onCheckedChange={(checked) => setValue("forRent", checked)} />
                <Label htmlFor="forRent" className="font-normal">
                  For Rent
                </Label>
              </div>
              <div className="flex items-center gap-2">
                <Switch id="forSale" checked={forSale} onCheckedChange={(checked) => setValue("forSale", checked)} />
                <Label htmlFor="forSale" className="font-normal">
                  For Sale
                </Label>
              </div>
            </div>
          </div>

          {/* Status */}
          <div className="space-y-2">
            <Label htmlFor="status">Status</Label>
            <Select value={watch("status")} onValueChange={(value) => setValue("status", value as ListingStatus)}>
              <SelectTrigger>
                <SelectValue placeholder="Select status" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="active">Active</SelectItem>
                <SelectItem value="pending">Pending</SelectItem>
                <SelectItem value="inactive">Inactive</SelectItem>
                <SelectItem value="sold">Sold</SelectItem>
                <SelectItem value="rented">Rented</SelectItem>
              </SelectContent>
            </Select>
          </div>

          {/* Amenities */}
          <div className="space-y-2">
            <Label>Amenities</Label>
            <div className="flex flex-wrap gap-2">
              {AVAILABLE_AMENITIES.map((amenity) => (
                <Badge
                  key={amenity}
                  variant={selectedAmenities.includes(amenity) ? "default" : "outline"}
                  className={cn(
                    "cursor-pointer transition-colors",
                    selectedAmenities.includes(amenity) ? "bg-primary hover:bg-primary/80" : "hover:bg-muted",
                  )}
                  onClick={() => toggleAmenity(amenity)}
                >
                  {amenity}
                  {selectedAmenities.includes(amenity) && <X className="ml-1 h-3 w-3" />}
                </Badge>
              ))}
            </div>
          </div>

          {/* Submit Button */}
          <div className="flex gap-3 pt-4">
            <Button
              type="button"
              variant="outline"
              className="flex-1 bg-transparent"
              onClick={() => onOpenChange(false)}
            >
              Cancel
            </Button>
            <Button type="submit" className="flex-1">
              {editListing ? "Update Listing" : "Create Listing"}
            </Button>
          </div>
        </form>
      </SheetContent>
    </Sheet>
  )
}
