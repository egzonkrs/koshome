import * as z from "zod"

export const loginSchema = z.object({
  email: z.string().min(1, { message: "Email is required" }).email({ message: "Please enter a valid email address" }),
  password: z
    .string()
    .min(1, { message: "Password is required" })
    .min(8, { message: "Password must be at least 8 characters" }),
  rememberMe: z.boolean().optional(),
})

export type LoginFormValues = z.infer<typeof loginSchema>

export const signupSchema = z
  .object({
    name: z
      .string()
      .min(1, { message: "Name is required" })
      .min(2, { message: "Name must be at least 2 characters" })
      .max(50, { message: "Name must be less than 50 characters" })
      .regex(/^[a-zA-Z\s'-]+$/, { message: "Name can only contain letters, spaces, hyphens, and apostrophes" }),
    email: z
      .string()
      .min(1, { message: "Email is required" })
      .email({ message: "Please enter a valid email address" })
      .max(100, { message: "Email must be less than 100 characters" }),
    password: z
      .string()
      .min(1, { message: "Password is required" })
      .min(8, { message: "Password must be at least 8 characters" })
      .max(100, { message: "Password must be less than 100 characters" })
      .regex(/[A-Z]/, { message: "Password must contain at least one uppercase letter" })
      .regex(/[a-z]/, { message: "Password must contain at least one lowercase letter" })
      .regex(/[0-9]/, { message: "Password must contain at least one number" })
      .regex(/[!@#$%^&*(),.?":{}|<>]/, { message: "Password must contain at least one special character" }),
    confirmPassword: z.string().min(1, { message: "Please confirm your password" }),
    terms: z.boolean().refine((val) => val === true, {
      message: "You must agree to the terms and conditions",
    }),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match",
    path: ["confirmPassword"],
  })

export type SignupFormValues = z.infer<typeof signupSchema>

export const contactSchema = z.object({
  name: z
    .string()
    .min(1, { message: "Name is required" })
    .min(2, { message: "Name must be at least 2 characters" })
    .max(50, { message: "Name must be less than 50 characters" }),
  email: z.string().min(1, { message: "Email is required" }).email({ message: "Please enter a valid email address" }),
  phone: z
    .string()
    .optional()
    .refine((val) => !val || /^[+]?[(]?[0-9]{1,4}[)]?[-\s./0-9]*$/.test(val), {
      message: "Please enter a valid phone number",
    }),
  subject: z
    .string()
    .min(1, { message: "Subject is required" })
    .min(5, { message: "Subject must be at least 5 characters" })
    .max(100, { message: "Subject must be less than 100 characters" }),
  message: z
    .string()
    .min(1, { message: "Message is required" })
    .min(10, { message: "Message must be at least 10 characters" })
    .max(1000, { message: "Message must be less than 1000 characters" }),
})

export type ContactFormValues = z.infer<typeof contactSchema>

export const propertyInquirySchema = z.object({
  name: z.string().min(1, { message: "Name is required" }).min(2, { message: "Name must be at least 2 characters" }),
  email: z.string().min(1, { message: "Email is required" }).email({ message: "Please enter a valid email address" }),
  phone: z
    .string()
    .min(1, { message: "Phone number is required" })
    .refine((val) => /^[+]?[(]?[0-9]{1,4}[)]?[-\s./0-9]*$/.test(val), {
      message: "Please enter a valid phone number",
    }),
  message: z
    .string()
    .min(1, { message: "Message is required" })
    .min(10, { message: "Message must be at least 10 characters" }),
  preferredContact: z.enum(["email", "phone", "both"], {
    required_error: "Please select a preferred contact method",
  }),
  scheduleTour: z.boolean().optional(),
  tourDate: z.string().optional(),
})

export type PropertyInquiryFormValues = z.infer<typeof propertyInquirySchema>
