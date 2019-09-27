export class Daterange implements IDaterange {
    begin: Date;
    beginString?: string;
    end: Date;
    endString?: string;

    constructor(newBegin: Date, newEnd: Date) {
        this.begin = newBegin;
        this.beginString = '';
        this.endString = '';
        this.end = newEnd;
    }
}

export interface IDaterange {
    begin: Date;
    end: Date;
}