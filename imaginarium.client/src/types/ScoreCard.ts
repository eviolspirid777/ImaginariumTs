import type { Card } from "./Card";

export type ScoreCard = {
    score: number | undefined;
    isLeader: boolean | undefined;
    card: Card | undefined;
    name: string[] | undefined;
    owner: string | undefined
} | undefined