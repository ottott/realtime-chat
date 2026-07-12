<script setup>
import { ref } from "vue";
import { login } from "../api/api";
import { useAuthStore } from "../stores/auth";
import { useRouter } from "vue-router";
import AppButton from "../components/AppButton.vue";

const router = useRouter();
const auth = useAuthStore();
const email = ref("");
const password = ref("");

async function handleLogin() {
    try {
        const result = await login(email.value, password.value);

        auth.setToken(result.token);
        router.push("/chat");

        console.log(auth.token);

        alert("Login successful!");
    } catch (err) {
        alert(err.message);
    }
}
</script>

<template>
    <h1>Login</h1>

    <div>
        <input
            v-model="email"
            type="email"
            placeholder="Email"
        />
    </div>

    <br>

    <div>
        <input
            v-model="password"
            type="password"
            placeholder="Password"
        />
    </div>

    <br>

    <AppButton @click="handleLogin">
        Login
    </AppButton>

    <br>

    <p>
    Don't have an account?
    <RouterLink to="/register">Register</RouterLink>
    </p>
</template>