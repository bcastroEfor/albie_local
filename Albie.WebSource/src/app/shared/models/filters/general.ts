export class GeneralFilter {
    year: number;
    week: number;
    dateFromString: string = '';
    dateToString: string = '';
    dateFrom: Date;
    dateTo: Date;

    constructor(dateFrom?: Date, dateTo?: Date) {
        const now = new Date();
        this.year = now.getFullYear();
        this.week = this.getWeek(now);
        
        if (dateFrom != null) {
            this.dateFrom = dateFrom;
            this.year = dateFrom.getFullYear();
            this.week = this.getWeek(dateFrom);
        }
        if (dateTo != null) {
            this.dateTo = dateTo;
        }
    }

    private getWeek = function (targetDate: any) {
        const onejan: any = new Date(targetDate.getFullYear(), 0, 1);
        return Math.ceil((((targetDate - onejan) / 86400000) + onejan.getDay() + 1) / 7);
    }
}
