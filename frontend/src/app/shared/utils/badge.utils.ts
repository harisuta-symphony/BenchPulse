export function skillStatusClass(status: string | number): string {
  return (['badge-learning', 'badge-proficient', 'badge-expert'] as const)[Number(status)] ?? 'badge-learning';
}

export function skillStatusLabel(status: string | number): string {
  return (['Learning', 'Proficient', 'Expert'] as const)[Number(status)] ?? 'Learning';
}

export function bookingStatusClass(status: string | number): string {
  return (['badge-pending', 'badge-confirmed', 'badge-cancelled', 'badge-completed'] as const)[Number(status)] ?? 'badge-pending';
}

export function bookingStatusLabel(status: string | number): string {
  return (['Pending', 'Confirmed', 'Cancelled', 'Completed'] as const)[Number(status)] ?? 'Pending';
}
