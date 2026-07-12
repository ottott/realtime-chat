<script setup>
import { ref, onMounted } from "vue";

import { getMessages } from "../api/api";
import { useAuthStore } from "../stores/auth";
import { useRouter } from "vue-router";
import { connect, sendMessage, disconnect } from "../services/chat";
import { nextTick } from "vue";

const auth = useAuthStore();

const messages = ref([]);
const newMessage = ref("");
const messagesContainer = ref(null);

const router = useRouter();

onMounted(async () => {

    messages.value = await getMessages(auth.token);


    await scrollToBottom();

    await connect(auth.token, async (username, message) => {

        messages.value.push({
            username,
            content: message
        });

        await scrollToBottom();

    });

});

async function send() {

    if (!newMessage.value.trim()) {
        return;
    }

    await sendMessage(newMessage.value);

    newMessage.value = "";

}

async function logout() {

    await disconnect();

    auth.logout();

    router.push("/login");

}

async function scrollToBottom() {

    await nextTick();
    
    if (messagesContainer.value) {
        messagesContainer.value.scrollTop =
            messagesContainer.value.scrollHeight;
    }

}

</script>

<template>

    <button @click="logout">
        Logout
    </button>

    <h1>Chat</h1>

    <div ref="messagesContainer" style="height:300px; overflow-y:auto;">

        <div v-for="message in messages" :key="message.id">
            {{ message.username }}:
            {{ message.content }}
        </div>

    </div>

    <br>

    <input v-model="newMessage" @keyup.enter="send" placeholder="Type a message...">

    <button @click="send">
        Send
    </button>

</template>