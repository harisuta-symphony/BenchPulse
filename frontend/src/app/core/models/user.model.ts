export interface User {
  id: string;
  fullName: string;
  email: string;
  avatarUrl?: string;
  bio?: string;
  department?: string;
  createdAt: string;
}

export interface CreateUser {
  fullName: string;
  email: string;
  bio?: string;
  department?: string;
}

export interface UpdateUser {
  fullName?: string;
  bio?: string;
  department?: string;
  avatarUrl?: string;
}