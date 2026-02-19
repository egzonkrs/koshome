/**
 * Apartment Query Hooks
 */

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { queryKeys } from "./query-keys"
import { apartmentProvider } from "../api/providers"
import type { ApartmentFilterParams, CreateApartmentRequest } from "../api/types"

export function useApartments(params?: ApartmentFilterParams) {
  return useQuery({
    queryKey: queryKeys.apartments.list(params || {}),
    queryFn: () => apartmentProvider.getApartments(params),
  })
}

export function useApartment(id: string) {
  return useQuery({
    queryKey: queryKeys.apartments.detail(id),
    queryFn: () => apartmentProvider.getApartmentById(id),
    enabled: !!id,
  })
}

export function useCreateApartment() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (data: CreateApartmentRequest) => apartmentProvider.createApartment(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: queryKeys.apartments.lists() })
    },
  })
}
