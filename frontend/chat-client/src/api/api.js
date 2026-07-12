const API_URL = "http://localhost:5122";

export async function login(email, password) {
    const response = await fetch(`${API_URL}/login`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            email,
            password
        })
    });

    if (!response.ok) {
        throw new Error("Invalid email or password");
    }

    return await response.json();
}

export async function register(username, email, password) {
    const response = await fetch(`${API_URL}/users`, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({
            username,
            email,
            password
        })
    });

    if (!response.ok) {
        throw new Error(await response.text());
    }

    return await response.json();
}


export async function getMessages(token) {
    const response = await fetch(`${API_URL}/messages`, {
        headers: {
            Authorization: `Bearer ${token}`
        }
    });

    if (!response.ok) {
        throw new Error("Failed to load messages");
    }

    return await response.json();
}

