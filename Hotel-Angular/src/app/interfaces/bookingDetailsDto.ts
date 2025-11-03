export interface BookingDetailsDto {
    id: number,
    startDate: Date,
    endDate: Date,
    status: string,
    price: number,
    personCount: number,
    comment: string,
    customer: any[],
    rooms: any[]
}
