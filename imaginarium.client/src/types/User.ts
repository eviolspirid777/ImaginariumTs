export type User = {
    id: number | undefined;
    isReady: boolean | undefined;
    name: string | undefined;
    score: number | undefined;
    cards: {
        id: number | undefined;
        cardUrl: string | undefined;
        cardName: string | undefined;
    } | undefined
} | undefined