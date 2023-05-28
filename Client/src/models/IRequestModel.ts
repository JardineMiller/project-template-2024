import type { IModel } from "@/models/base/IModel";

export default interface IRequestModel<T extends IModel, K> {
    toRequest(): K;
}
