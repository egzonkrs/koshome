"use client"

import type React from "react"

import { useState } from "react"
import { useRouter, useSearchParams } from "next/navigation"
import { useApartmentStore } from "@/store/apartment-store"
import { Button } from "@/components/ui/button"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Slider } from "@/components/ui/slider"
import { Switch } from "@/components/ui/switch"
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs"
import { Search, MapPin, Filter } from "lucide-react"

export function SearchBox() {
  const { filters, setFilters } = useApartmentStore()
  const [priceRange, setPriceRange] = useState<[number, number]>([0, 500000])
  const [searchQuery, setSearchQuery] = useState("")
  const router = useRouter()
  const searchParams = useSearchParams()

  const handleSearchChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setSearchQuery(e.target.value)
  }

  const handleSearchSubmit = (e: React.FormEvent) => {
    e.preventDefault()
    const newFilters = { search: searchQuery }
    setFilters(newFilters)
    // Update URL
    const urlParams = new URLSearchParams(searchParams.toString())
    if (searchQuery) {
      urlParams.set("search", searchQuery)
    } else {
      urlParams.delete("search")
    }
    router.push(`${window.location.pathname}?${urlParams.toString()}`, { scroll: false })
  }

  const handlePriceChange = (value: number[]) => {
    setPriceRange([value[0], value[1]])
    const newFilters = { priceMin: value[0], priceMax: value[1] }
    setFilters(newFilters)
  }

  const handleDistrictChange = (value: string) => {
    const newFilters = { district: value }
    setFilters(newFilters)
  }

  const handleBedroomsChange = (value: string) => {
    const newFilters = { bedrooms: Number.parseInt(value) }
    setFilters(newFilters)
  }

  const handleForSaleChange = (checked: boolean) => {
    const newFilters = { forSale: checked }
    setFilters(newFilters)
  }

  const handleForRentChange = (checked: boolean) => {
    const newFilters = { forRent: checked }
    setFilters(newFilters)
  }

  const districts = [
    "City Center",
    "Arbëri",
    "Dardania",
    "Kalabria",
    "Old Town",
    "Ulpiana",
    "Veternik",
    "Bregu i Diellit",
  ]

  return (
    <div className="bg-[#eeefe9] dark:bg-background rounded-t-3xl shadow-lg p-6 mx-auto max-w-7xl">
      <Tabs defaultValue="search" className="w-full tabs-darker">
        <TabsList className="grid grid-cols-2 mb-6 bg-[#e5e6e1]">
          <TabsTrigger
            value="search"
            className="text-base data-[state=active]:bg-[#eeefe9] data-[state=inactive]:bg-[#e5e6e1]"
          >
            <Search className="mr-2 h-4 w-4" />
            Search
          </TabsTrigger>
          <TabsTrigger
            value="filters"
            className="text-base data-[state=active]:bg-[#eeefe9] data-[state=inactive]:bg-[#e5e6e1]"
          >
            <Filter className="mr-2 h-4 w-4" />
            Filters
          </TabsTrigger>
        </TabsList>

        <TabsContent value="search" className="space-y-6">
          <form onSubmit={handleSearchSubmit} className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 h-5 w-5 text-muted-foreground" />
            <Input
              placeholder="Search by location, district..."
              className="pl-10 h-12 text-lg"
              value={searchQuery}
              onChange={handleSearchChange}
            />
            <Button type="submit" className="absolute right-1 top-1 h-10">
              Search
            </Button>
          </form>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="space-y-2">
              <Label htmlFor="district">District</Label>
              <Select onValueChange={handleDistrictChange} value={filters.district}>
                <SelectTrigger id="district">
                  <SelectValue placeholder="Any district" />
                </SelectTrigger>
                <SelectContent>
                  {districts.map((district) => (
                    <SelectItem key={district} value={district}>
                      {district}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="bedrooms">Bedrooms</Label>
              <Select onValueChange={handleBedroomsChange} value={filters.bedrooms?.toString()}>
                <SelectTrigger id="bedrooms">
                  <SelectValue placeholder="Any bedrooms" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="0">Studio</SelectItem>
                  <SelectItem value="1">1+ Bedroom</SelectItem>
                  <SelectItem value="2">2+ Bedrooms</SelectItem>
                  <SelectItem value="3">3+ Bedrooms</SelectItem>
                  <SelectItem value="4">4+ Bedrooms</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label>Property Type</Label>
              <div className="flex items-center justify-between gap-4 pt-2">
                <div className="flex items-center gap-2">
                  <Switch id="sale-switch" checked={filters.forSale} onCheckedChange={handleForSaleChange} />
                  <Label htmlFor="sale-switch" className="text-sm cursor-pointer">
                    For Sale
                  </Label>
                </div>
                <div className="flex items-center gap-2">
                  <Switch id="rent-switch" checked={filters.forRent} onCheckedChange={handleForRentChange} />
                  <Label htmlFor="rent-switch" className="text-sm cursor-pointer">
                    For Rent
                  </Label>
                </div>
              </div>
            </div>
          </div>
        </TabsContent>

        <TabsContent value="filters" className="space-y-6">
          <div className="space-y-4">
            <div className="flex items-center justify-between">
              <Label>Price Range</Label>
              <div className="text-sm text-muted-foreground">
                {priceRange[0].toLocaleString()} € - {priceRange[1].toLocaleString()} €
              </div>
            </div>
            <Slider
              defaultValue={[0, 500000]}
              max={500000}
              step={5000}
              value={[priceRange[0], priceRange[1]]}
              onValueChange={handlePriceChange}
              className="py-4 range-slider"
            />
          </div>

          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="space-y-2">
              <Label htmlFor="property-type">Property Type</Label>
              <Select>
                <SelectTrigger id="property-type">
                  <SelectValue placeholder="Any type" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="apartment">Apartment</SelectItem>
                  <SelectItem value="house">House</SelectItem>
                  <SelectItem value="villa">Villa</SelectItem>
                  <SelectItem value="penthouse">Penthouse</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="bathrooms">Bathrooms</Label>
              <Select>
                <SelectTrigger id="bathrooms">
                  <SelectValue placeholder="Any bathrooms" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="1">1+ Bathroom</SelectItem>
                  <SelectItem value="2">2+ Bathrooms</SelectItem>
                  <SelectItem value="3">3+ Bathrooms</SelectItem>
                </SelectContent>
              </Select>
            </div>

            <div className="space-y-2">
              <Label htmlFor="features">Features</Label>
              <Select>
                <SelectTrigger id="features">
                  <SelectValue placeholder="Any features" />
                </SelectTrigger>
                <SelectContent>
                  <SelectItem value="parking">Parking</SelectItem>
                  <SelectItem value="balcony">Balcony</SelectItem>
                  <SelectItem value="garden">Garden</SelectItem>
                  <SelectItem value="elevator">Elevator</SelectItem>
                  <SelectItem value="air-conditioning">Air Conditioning</SelectItem>
                </SelectContent>
              </Select>
            </div>
          </div>

          <div className="pt-4">
            <Button className="w-full">Apply Filters</Button>
          </div>
        </TabsContent>
      </Tabs>

      <div className="mt-6 flex items-center justify-center">
        <div className="flex items-center text-sm text-muted-foreground">
          <MapPin className="h-4 w-4 mr-1" />
          <span>8 properties found in selected area</span>
        </div>
      </div>
    </div>
  )
}
