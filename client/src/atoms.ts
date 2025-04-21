import {atom} from 'jotai';

export const JwtAtom = atom<string>(localStorage.getItem('jwt') || '')
