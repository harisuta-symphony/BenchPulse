export interface Booking {
    id: string;
    requesterId: string;
    requesterName: string;
    providerId: string;
    providerName: string;
    skillId: string;
    skillName: string;
    scheduledAt: string;
    durationMinutes: number;
    status: string;
    message?: string;
    meetingLink?: string;

}

export interface CreateBooking {
    requesterId: string;
    providerId: string;
    skillId: string;
    scheduledAt: string;
    durationMinutes: number;
    message?: string;
}

export interface UpdateBookingStatus {
    status: string;
    meetingLink?: string;
}
