import { roomDto } from "./roomDto";

export interface UpdateBookingRequest {
    id: number,
    startDate: Date,
    endDate: Date,
    totalPrice: number,
    personCount: number,
    comment: string,
    fullName: string,
    email: string,
    phoneNumber: string,
    roomIds: number[],
}
