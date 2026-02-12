import type { AxiosInstance, AxiosRequestConfig, AxiosResponse, CancelTokenSource } from 'axios'
import axios from 'axios'

export interface HttpClientOptions {
  baseURL: string
  defaultHeaders?: Record<string, string>
  getToken?: () => string | undefined
}
interface RequestError {
  canceled: boolean
  original?: unknown
}

export const safeRequest = async <T>(
  fn: () => Promise<AxiosResponse<T>>,
): Promise<[T | null, RequestError | null]> => {
  try {
    const res = await fn()
    return [res.data, null]
  } catch (err) {
    const canceled = axios.isCancel(err)
    return [null, { canceled, original: err }]
  }
}

export class HttpClient {
  private readonly api: AxiosInstance
  private readonly getToken?: () => string | undefined | null

  constructor({ baseURL, defaultHeaders = {}, getToken }: HttpClientOptions) {
    this.api = axios.create({
      baseURL,
      headers: {
        'Content-Type': 'application/json',
        ...defaultHeaders,
      },
    })
    this.getToken = getToken
  }
  get instance(): AxiosInstance {
    return this.api
  }
  private withAuth(config: AxiosRequestConfig = {}): AxiosRequestConfig {
    const token = this.getToken?.()
    return {
      ...config,
      headers: {
        ...config.headers,
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
    }
  }

  async get<T>(
    url: string,
    config: AxiosRequestConfig = {},
    params?: Record<string, unknown>,
  ): Promise<AxiosResponse<T>> {
    const finalConfig = {
      ...this.withAuth(config),
      params: { ...config.params, ...params },
    }
    return this.api.get<T>(url, finalConfig)
  }

  async post<T>(
    url: string,
    data: unknown,
    config: AxiosRequestConfig = {},
  ): Promise<AxiosResponse<T>> {
    return this.api.post<T>(url, data, this.withAuth(config))
  }

  async put<T>(
    url: string,
    data: unknown,
    config: AxiosRequestConfig = {},
  ): Promise<AxiosResponse<T>> {
    return this.api.put<T>(url, data, this.withAuth(config))
  }

  async delete<T>(url: string, config: AxiosRequestConfig = {}): Promise<AxiosResponse<T>> {
    return this.api.delete<T>(url, this.withAuth(config))
  }

  async upload<T>(
    url: string,
    formData: FormData,
    config: AxiosRequestConfig = {},
  ): Promise<AxiosResponse<T>> {
    return this.api.post<T>(url, formData, {
      ...this.withAuth(config),
      headers: {
        ...config.headers,
        'Content-Type': 'multipart/form-data',
      },
    })
  }

  createCancelToken(): CancelTokenSource {
    return axios.CancelToken.source()
  }
}
