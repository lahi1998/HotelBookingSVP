export interface CleaningScheduleDto {
    id: number;
    cleaningDate: Date;
    cleaned: boolean;
    roomNumber: number;
    roomFloor: number;
}
