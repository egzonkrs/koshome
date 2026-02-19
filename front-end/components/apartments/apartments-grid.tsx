"use client"
import type { Apartment } from "@/lib/types"
import { ApartmentCard } from "../apartment-card"

interface ApartmentsGridProps {
  apartments: Apartment[]
}

export function ApartmentsGrid({ apartments }: ApartmentsGridProps) {
  if (apartments.length === 0) {
    return (
      <div className="flex flex-col items-center justify-center py-12 text-center">
        <h3 className="text-xl font-semibold mb-2">No apartments found</h3>
        <p className="text-muted-foreground">Try adjusting your filters to find more properties</p>
      </div>
    )
  }

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
      {apartments.map((apartment) => (
        <ApartmentCard key={apartment.id} apartment={apartment} />
      ))}
    </div>
  )
}
