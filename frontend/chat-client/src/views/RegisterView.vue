<script setup>
import { ref } from "vue";
import { register } from "../api/api";
import { useRouter } from "vue-router";
import AppButton from "../components/AppButton.vue";

const router = useRouter();
const username = ref("");
const email = ref("");
const password = ref("");

async function registerUser() {
    try {
        await register(username.value, email.value, password.value);

        router.push("/login");


        alert("Registration successful!");
    } catch (err) {
        alert(err.message);
    }
}
</script>

<template>
    <h1>Register</h1>

    <div>
        <input
            v-model="username"
            type="text"
            placeholder="Username"
        />
    </div>

    <br>

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

    <AppButton @click="registerUser">
        Register
    </AppButton>

    <br> 

    <p>
    Already have an account?
    <RouterLink to="/login">Login</RouterLink>
    </p>
</template>