import axios from "axios"

const baseURL = 'http://localhost:5276/api/User';

export const playersRequest = {
    userGet: async (req: String) => {
        return (await axios.get(`${baseURL}/${req}`)).data
    },
    userPost: async (req: String) => {
        return (await axios.post(`${baseURL}/${req}`)).data
    },
    userPostResponse: async(req: String) => {
        return (await axios.post(`${baseURL}/${req}`))
    }
}