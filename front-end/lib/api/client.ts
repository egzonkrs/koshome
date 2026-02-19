import axios, { AxiosError } from "axios"
import { config } from "../config"
import type { ApiError } from "./types"

const getAccessToken = () => {
  if (typeof window === "undefined") return null
  return localStorage.getItem(config.auth.tokenKey)
}

export const apiClient = axios.create({
  baseURL: config.api.url,
  headers: {
    "Content-Type": "application/json",
  },
})

apiClient.interceptors.request.use((request) => {
  const token = getAccessToken()
  if (token) {
    request.headers.Authorization = `Bearer ${token}`
  }
  return request
})

apiClient.interceptors.response.use(
  (response) => response,
  (error: AxiosError<ApiError>) => {
    if (error.response?.status === 401 && typeof window !== "undefined") {
      localStorage.removeItem(config.auth.tokenKey)
      localStorage.removeItem(config.auth.refreshTokenKey)
      localStorage.removeItem(config.auth.userKey)
    }

    const payload = error.response?.data as any
    const errorMessages =
      payload?.errors && typeof payload.errors === "object" ? Object.values(payload.errors).filter(Boolean) : []
    const apiError: ApiError = {
      code: payload?.code || (payload?.errors ? Object.keys(payload.errors)[0] : "UNKNOWN"),
      message: payload?.message || (errorMessages[0] as string) || error.message || "Unexpected error",
      field: payload?.field,
    }

    return Promise.reject(apiError)
  },
)
