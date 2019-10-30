export class Rate {
    constructor(
        public id?: number,
        public ask?: number,
        public bid?: number,
        public currency?: string,
        public pair?: string,
        public dateUpdate?: Date) { }
}