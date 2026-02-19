"use client"

import { useRouter, usePathname, useSearchParams } from "next/navigation"
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from "@/components/ui/select"
import { Label } from "@/components/ui/label"
import { useApartmentStore } from "@/store/apartment-store"
import { useEffect } from "react"

export function ApartmentsSorting() {
  const { sortBy, setSortBy } = useApartmentStore()
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  // Initialize sort option from URL
  useEffect(() => {
    const sort = searchParams.get("sort")
    if (sort && isValidSortOption(sort)) {
      setSortBy(sort as typeof sortBy)
    }
  }, [searchParams, setSortBy])

  function isValidSortOption(value: string): boolean {
    return ["price-asc", "price-desc", "area-asc", "area-desc", "newest", "oldest"].includes(value)
  }

  const handleSortChange = (value: string) => {
    if (!isValidSortOption(value)) return

    setSortBy(value as typeof sortBy)

    // Update URL
    const params = new URLSearchParams(searchParams.toString())
    params.set("sort", value)
    params.set("page", "1") // Reset to page 1 when sorting changes
    router.push(`${pathname}?${params.toString()}`, { scroll: false })
  }

  return (
    <div className="flex items-center gap-2">
      <Label htmlFor="sort-select" className="text-sm whitespace-nowrap">
        Sort by:
      </Label>
      <Select value={sortBy} onValueChange={handleSortChange}>
        <SelectTrigger id="sort-select" className="w-[180px]">
          <SelectValue placeholder="Sort by" />
        </SelectTrigger>
        <SelectContent>
          <SelectItem value="newest">Newest first</SelectItem>
          <SelectItem value="oldest">Oldest first</SelectItem>
          <SelectItem value="price-asc">Price: Low to High</SelectItem>
          <SelectItem value="price-desc">Price: High to Low</SelectItem>
          <SelectItem value="area-asc">Area: Small to Large</SelectItem>
          <SelectItem value="area-desc">Area: Large to Small</SelectItem>
        </SelectContent>
      </Select>
    </div>
  )
}
