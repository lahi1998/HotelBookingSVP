import { CustomerDto } from "./customerDto";

export interface BookingListItemDto {
    id: number,
    roomCount: number,
    startDate: Date,
    endDate: Date,
    customer: CustomerDto,
}