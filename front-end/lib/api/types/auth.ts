/**
 * Authentication Types
 */

export interface LoginRequest {
  email: string
  password: string
}

export interface RegisterRequest {
  firstName: string
  lastName: string
  email: string
  password: string
}

export interface AuthResponse {
  accessToken?: string
  refreshToken?: string
  expiresIn?: number
  tokenType?: string
  // User info may be included
  userId?: string
  firstName?: string
  lastName?: string
  roles?: string[]
}

export interface CurrentUser {
  userId: string
  email: string
  firstName: string
  lastName: string
  roles: string[]
}
