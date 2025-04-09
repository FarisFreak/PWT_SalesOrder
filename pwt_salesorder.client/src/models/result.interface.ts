export interface IResult<T> {
  status: boolean
  message?: string
  data?: T | Array<T> | any
}
