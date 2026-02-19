"use client"

import { useApartmentStore } from "@/store/apartment-store"
import { ApartmentCard } from "./apartment-card"

export function ApartmentList() {
  const { filteredApartments } = useApartmentStore()

  return (
    <div className="h-full overflow-y-auto p-4">
      <div className="mb-4">
        <h2 className="text-xl font-semibold">{filteredApartments.length} Properties Available</h2>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 gap-6">
        {filteredApartments.map((apartment) => (
          <ApartmentCard key={apartment.id} apartment={apartment} />
        ))}

        {filteredApartments.length === 0 && (
          <div className="text-center py-12 col-span-2">
            <p className="text-muted-foreground">No properties match your search criteria.</p>
          </div>
        )}
      </div>
    </div>
  )
}
