import { CustomerDto } from "./customerDto";
import { roomDto } from "./roomDto";

export interface BookingDetailsDto {
    checkInStatus: string,
    id: number,
    startDate: Date,
    endDate: Date,
    status: string,
    price: number,
    personCount: number,
    comment: string,
    customer: CustomerDto,
    rooms: roomDto[],
    totalPrice: number
}
