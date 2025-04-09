import moment from "moment";

export const Format = {
    Currency : (value?: number | null, valuta: string | null = 'IDR') : string => {
        if (value === null || isNaN(value)) return 'Rp 0';
        return new Intl.NumberFormat('id-ID', {
            style: 'currency',
            currency: valuta,
            minimumFractionDigits: 0
        }).format(value);
    },
    Date : (param: { date?: Date | string; fullDate?: boolean; useTime?: boolean; separator?: string }): string => {
        const _date = param.date ?? new Date();
        let _separator = param.separator ?? '-';
        let _format = `DD${_separator}MM${_separator}YYYY`;

        if (param.fullDate) {
            _separator = ' ';
            _format = `DD${_separator}MMMM${_separator}YYYY`;
        }

        if (param.useTime) _format = `${_format} - HH:mm`;

        return moment(_date).format(_format);
    },
    Capitalize : (s: string) => (s && String(s.toLowerCase()[0]).toUpperCase() + String(s.toLowerCase()).slice(1)) || ""
}
