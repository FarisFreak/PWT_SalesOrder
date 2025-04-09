
export const fetchWrapper = {
	get: request('GET'),
	post: request('POST'),
	put: request('PUT'),
	delete: request('DELETE'),
	patch: request('PATCH')
};

function request(method: string) {
	return (url: string, body?: unknown) => {
		const requestOptions: RequestInit = {
			method,
			headers: authHeader(body)
		};
		if (body) {
			if (body instanceof FormData) {
				requestOptions.body = body;
			} else {
				requestOptions.body = JSON.stringify(body);
			}
		}
		return fetch(url, requestOptions).then(handleResponse);
	};
}

function authHeader(body: unknown): HeadersInit {
	const options: Record<string, string> | HeadersInit = {};

	if (!(body instanceof FormData)) {
		options['Content-Type'] = 'application/json';
	}
	return options;
}

async function handleResponse(response: Response) {
	const isJson = response.headers?.get('content-type')?.includes('application/json');
	const data = isJson ? await response.json() : null;
	data.code = response.status;
	return data;
}
