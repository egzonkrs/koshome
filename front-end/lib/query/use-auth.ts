/**
 * Authentication Query Hooks
 */

import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query"
import { queryKeys } from "./query-keys"
import { authProvider } from "../api/providers"
import type { LoginRequest, RegisterRequest } from "../api/types"

export function useCurrentUser() {
  return useQuery({
    queryKey: queryKeys.auth.user(),
    queryFn: () => authProvider.getCurrentUser(),
    staleTime: 1000 * 60 * 10,
    retry: false,
  })
}

export function useLogin() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (data: LoginRequest) => authProvider.login(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: queryKeys.auth.user() })
    },
  })
}

export function useRegister() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: (data: RegisterRequest) => authProvider.register(data),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: queryKeys.auth.user() })
    },
  })
}

export function useLogout() {
  const queryClient = useQueryClient()

  return useMutation({
    mutationFn: () => authProvider.logout(),
    onSuccess: () => {
      queryClient.clear()
    },
  })
}
