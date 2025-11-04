import { roomTypeDto } from "./roomTypeDto";

export interface roomDto {
id: number;
roomType: roomTypeDto;
lastCleaned: Date;
number: number;
floor: number;
bedAmount: number;
roomstatus: string;
}
