import type { Card } from "./Card";

export type User = {
    id: number | undefined;
    isReady: boolean | undefined;
    name: string | undefined;
    score: number | undefined;
    cards: Card[] | undefined
} | undefined